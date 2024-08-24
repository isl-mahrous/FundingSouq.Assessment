using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Query to retrieve a paged list of clients with optional filtering and sorting.
/// </summary>
/// <remarks>
/// The result of the query is encapsulated in a <see cref="Result{T}"/> object. Where T is a <see cref="PagedResult{T}"/> of <see cref="ClientDto"/>.
/// </remarks>
public class ClientsAsPagedQuery : IRequest<Result<PagedResult<ClientDto>>>
{
    /// <summary>
    /// Gets or sets the ID of the user making the request.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the search key used to filter clients.
    /// </summary>
    public string SearchKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the page number to retrieve.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the key by which to sort the clients.
    /// </summary>
    public string SortKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the direction of sorting.
    /// </summary>
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}