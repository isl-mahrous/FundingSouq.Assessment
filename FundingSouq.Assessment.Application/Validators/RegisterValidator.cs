using FluentValidation;
using FundingSouq.Assessment.Core.Entities;

namespace FundingSouq.Assessment.Application.Validators;

public class RegisterValidator : AbstractValidator<Client>
{
    public RegisterValidator()
    {
        RuleFor(s=>s.Email).NotEmpty().WithMessage("Email is required.");
    }
}