using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;
using RedLockNet;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="RegisterHubUserCommand"/>.
/// </summary>
/// <remarks>
/// This handler registers a new HubUser, ensures that the email is unique,
/// hashes the user's password, and generates a JWT token upon successful registration.
/// It uses distributed locking to ensure that concurrent registration attempts for the same email do not cause issues.
/// </remarks>
public class RegisterHubUserCommandHandler : IRequestHandler<RegisterHubUserCommand, Result<HubUserLoginDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IDistributedLockFactory _redLockFactory;

    public RegisterHubUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService,
        IDistributedLockFactory redLockFactory)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _redLockFactory = redLockFactory;
    }

    public async Task<Result<HubUserLoginDto>> Handle(RegisterHubUserCommand request,
        CancellationToken cancellationToken)
    {
        // Define the resource key for locking to prevent concurrent registrations for the same email
        var resource = $"hub-user-registration-{request.Email.ToLower()}";
        var expiry = TimeSpan.FromSeconds(30);

        // Attempt to acquire the lock
        await using var redLock = await _redLockFactory.CreateLockAsync(resource, expiry);

        // If lock acquisition fails, return an error
        if (!redLock.IsAcquired)
        {
            return UserErrors.FailedToAcquireLock;
        }

        // Proceed with the registration if the lock is acquired
        var emailExists = await _unitOfWork.Users.ExistsAsync(i => i.Email == request.Email);
        if (emailExists)
            return UserErrors.EmailInUse;

        // Start a transaction to ensure atomicity
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var hashedPassword = _passwordHasher.Hash(request.Password);

        // Create a new HubUser with the provided details
        var user = new HubUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = hashedPassword,
            UserType = UserType.HubUser,
            Role = request.Role,
        };

        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        // Generate a JWT token for the registered user
        var jwt = _jwtService.GenerateToken(user.GetClaims());
        var userLoginDto = new HubUserLoginDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserType = user.UserType,
            Role = user.Role,
            Token = jwt.Token,
            CreatedAt = user.CreatedAt,
            LastModifiedAt = user.LastModifiedAt,
        };

        return userLoginDto;
    }
}