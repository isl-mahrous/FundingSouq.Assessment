using FluentValidation;

namespace FundingSouq.Assessment.Application.Queries;

public class ClientsAsPagedQueryValidator : AbstractValidator<ClientsAsPagedQuery>
{
    public ClientsAsPagedQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("PageSize must be less than or equal to 100");
    }
}