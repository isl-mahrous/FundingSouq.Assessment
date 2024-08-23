using FluentValidation;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Application.Commands;

public class RegisterHubUserCommandValidator : AbstractValidator<RegisterHubUserCommand>
{
    public RegisterHubUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Role).IsInEnum();
    }
}