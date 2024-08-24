using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Query to retrieve the search history of a specific user for a specific HubPage.
/// </summary>
/// <remarks>
/// The result of this query is encapsulated in a <c>Result&lt;List&lt;SearchHistoryDto&gt;&gt;</c> object.
/// </remarks>
public class UserSearchHistoryQuery : IRequest<Result<List<SearchHistoryDto>>>
{
    /// <summary>
    /// The ID of the HubUser whose search history is being queried.
    /// </summary>
    public int HubUserId { get; set; }
    
    /// <summary>
    /// The key of the HubPage for which the search history is being queried. 
    /// If null or empty, retrieves search history for all HubPages.
    /// </summary>
    public string HubPageKey { get; set; }
}