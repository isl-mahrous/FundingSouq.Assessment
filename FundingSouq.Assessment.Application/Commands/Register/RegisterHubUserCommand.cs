using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

public class RegisterHubUserCommand : IRequest<Result<HubUserLoginDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public HubUserRole Role { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterHubUserCommand, Result<HubUserLoginDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<HubUserLoginDto>> Handle(RegisterHubUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _unitOfWork.Users.ExistsAsync(i => i.Email == request.Email);

        if (emailExists)
            return UserErrors.EmailInUse;
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var hashedPassword = _passwordHasher.Hash(request.Password);
        
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