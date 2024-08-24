using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="LoginHubUserCommand"/>.
/// </summary>
/// <remarks>
/// This handler validates the user's credentials and, if successful, generates a JWT token for the user.
/// </remarks>
public class LoginHubUserCommandHandler : IRequestHandler<LoginHubUserCommand, Result<HubUserLoginDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginHubUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<HubUserLoginDto>> Handle(LoginHubUserCommand request, CancellationToken cancellationToken)
    {
        // Attempt to retrieve the user based on the provided email
        var user = await _unitOfWork.HubUsers.GetFirstAsync(x => x.Email == request.Email);

        // Return an error if the user is not found
        if (user is null)
            return UserErrors.UserNotFound;

        // Verify the provided password against the stored password hash
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return UserErrors.InvalidPassword;

        // Generate a JWT token for the authenticated user
        var jwt = _jwtService.GenerateToken(user.GetClaims());

        // Prepare the login DTO with user details and the generated token
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