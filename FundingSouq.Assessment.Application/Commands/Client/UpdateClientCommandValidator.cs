using FluentValidation;
using FundingSouq.Assessment.Core.Extensions;

namespace FundingSouq.Assessment.Application.Commands;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage("Client Id is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .Must(Helper.IsValidEmail)
            .WithMessage("Invalid email address");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(60)
            .WithMessage("First name should not exceed 60 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(60)
            .WithMessage("Last name should not exceed 60 characters");

        RuleFor(x => x.PersonalId)
            .NotEmpty()
            .WithMessage("Personal ID is required")
            .Must(x => x is not null && x.Length == 11)
            .WithMessage("Personal ID should be 11 characters long");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .WithMessage("Mobile number is required")
            .Must(Helper.IsValidPhoneNumber)
            .WithMessage("Invalid phone number. Number should be in international format. e.g. +971501234567");

        RuleFor(x => x.ProfilePhoto)
            .Must(x => x == null || x.Length > 0)
            .WithMessage("Profile photo is invalid");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Gender should be 0 or 1");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("Country Id is required");

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessage("City Id is required");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required");
    }
}