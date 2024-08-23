using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

public class LoginHubUserCommand : IRequest<Result<HubUserLoginDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

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
        var user = await _unitOfWork.HubUsers.GetFirstAsync(x => x.Email == request.Email);

        if (user is null)
            return UserErrors.UserNotFound;

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return UserErrors.InvalidPassword;

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