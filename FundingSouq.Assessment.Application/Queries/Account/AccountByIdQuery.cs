using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Query to retrieve an account by its ID.
/// </summary>
/// <remarks>
/// The result of the query is encapsulated in a <see cref="Result{T}"/> object. where T is an <see cref="AccountDto"/>.
/// </remarks>
public class AccountByIdQuery : IRequest<Result<AccountDto>>
{
    /// <summary>
    /// Gets or sets the ID of the account to retrieve.
    /// </summary>
    public int Id { get; set; }
}