using FluentValidation;

namespace FundingSouq.Assessment.Application.Commands;

public class UpdateClientAccountCommandValidator : AbstractValidator<UpdateClientAccountCommand>
{
    public UpdateClientAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Account Id is required");

        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .WithMessage("Account number is required");

        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Account type valid values are in range 0 to 10");
    }
}