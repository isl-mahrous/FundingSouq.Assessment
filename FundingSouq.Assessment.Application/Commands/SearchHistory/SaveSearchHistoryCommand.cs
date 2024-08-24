using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to save a search history record for a HubUser.
/// </summary>
/// <remarks>
/// The result of the operation is encapsulated in a <see cref="Result"/> object.
/// </remarks>
public class SaveSearchHistoryCommand : IRequest<Result>
{
    /// <summary>
    /// Gets or sets the HubUser ID associated with the search history.
    /// </summary>
    public int HubUserId { get; set; }

    /// <summary>
    /// Gets or sets the key of the HubPage where the search was performed.
    /// </summary>
    public string HubPageKey { get; set; }

    /// <summary>
    /// Gets or sets the search query.
    /// </summary>
    public string SearchQuery { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the search was performed.
    /// </summary>
    public DateTime SearchDate { get; set; }
}