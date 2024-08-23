using FluentValidation;

namespace FundingSouq.Assessment.Application.Commands.Login;

public class LoginHubUserCommandValidator : AbstractValidator<LoginHubUserCommand>
{
    public LoginHubUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}