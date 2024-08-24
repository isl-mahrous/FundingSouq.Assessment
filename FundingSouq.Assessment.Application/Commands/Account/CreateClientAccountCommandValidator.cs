using FluentValidation;

namespace FundingSouq.Assessment.Application.Commands;

public class CreateClientAccountCommandValidator : AbstractValidator<CreateClientAccountCommand>
{
    public CreateClientAccountCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage("Client Id is required");
        
        RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number is required");

        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Account type valid values are in range 0 to 10");
    }
}