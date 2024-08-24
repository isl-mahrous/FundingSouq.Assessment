using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Handler for processing <see cref="UserSearchHistoryQuery"/>.
/// </summary>
/// <remarks>
/// This handler retrieves the search history for a specific HubUser, filtered by HubPage if specified.
/// The result is grouped by HubPage and returned as a list of <see cref="SearchHistoryDto"/> objects.
/// </remarks>
public class UserSearchHistoryQueryHandler : IRequestHandler<UserSearchHistoryQuery, Result<List<SearchHistoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserSearchHistoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the <see cref="UserSearchHistoryQuery"/> by retrieving the search history and grouping by HubPage.
    /// </summary>
    /// <param name="request">The query containing the user ID and optional HubPage key.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <c>Result&lt;List&lt;SearchHistoryDto&gt;&gt;</c> containing the search history grouped by HubPage.
    /// </returns>
    public async Task<Result<List<SearchHistoryDto>>> Handle(UserSearchHistoryQuery request,
        CancellationToken cancellationToken)
    {
        // Retrieve search history for the specified user, filtered by HubPage key if provided
        var searchHistoryList = await _unitOfWork.SearchHistories.GetAllAsync(
            s => s.HubUserId == request.HubUserId &&
                 (string.IsNullOrEmpty(request.HubPageKey) || s.HubPage.Key == request.HubPageKey),
            includes: s => s.HubPage);

        // Group search history by HubPage key and map to SearchHistoryDto objects
        var result = searchHistoryList
            .GroupBy(s => s.HubPage.Key)
            .Select(history => new SearchHistoryDto
            {
                HubPageId = history.First().HubPage.Id, // Get the HubPage ID from the first entry in the group
                HubPageKey = history.Key, // Use the grouped key as the HubPageKey
                History = history
                    .OrderByDescending(s => s.SearchDate) // Order search history by date
                    .Select(h => new HubPageHistoryDto
                    {
                        Id = h.Id,
                        SearchQuery = h.SearchQuery,
                        SearchDate = h.SearchDate
                    }).ToList()
            }).ToList();

        return result;
    }
}