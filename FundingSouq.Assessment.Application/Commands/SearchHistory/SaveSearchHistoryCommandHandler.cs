using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="SaveSearchHistoryCommand"/>.
/// </summary>
/// <remarks>
/// This handler saves or updates a search history record for a HubUser, ensuring that only the last three unique searches
/// are kept for each user-page combination. It also adds the HubPage record if it does not already exist.
/// </remarks>
public class SaveSearchHistoryCommandHandler : IRequestHandler<SaveSearchHistoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public SaveSearchHistoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SaveSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        // Retrieve or create the HubPage associated with the search
        var hubPage = await _unitOfWork.HubPages.GetFirstAsync(x => x.Key == request.HubPageKey);
        if (hubPage is null)
        {
            hubPage = new HubPage
            {
                Key = request.HubPageKey
            };
            _unitOfWork.HubPages.Add(hubPage);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        
        // Retrieve existing search history records for the user on the specified page
        var existingSearches = await _unitOfWork.SearchHistories
            .GetAllAsync(x => x.HubUserId == request.HubUserId && x.HubPageId == hubPage.Id);

        // Check if the search query already exists in the history
        var existingSearch = existingSearches.FirstOrDefault(x => x.SearchQuery == request.SearchQuery);
        if (existingSearch != null)
        {
            // If the query exists, update the search date to the current time
            existingSearch.SearchDate = DateTime.UtcNow;
        }
        else
        {
            // If the query does not exist, create a new search history record
            var newSearchHistory = new SearchHistory
            {
                HubUserId = request.HubUserId,
                HubPageId = hubPage.Id,
                SearchQuery = request.SearchQuery,
                SearchDate = DateTime.UtcNow
            };
            
            _unitOfWork.SearchHistories.Add(newSearchHistory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            existingSearches.Add(newSearchHistory);
        }

        // Keep only the last three unique searches, delete the older ones
        var lastUniqueSearches = existingSearches
            .OrderByDescending(x => x.SearchDate)
            .Take(3)
            .ToList();

        if (existingSearches.Count > 3)
        {
            var oldestSearches = existingSearches
                .Where(x => !lastUniqueSearches.Select(us => us.Id).Contains(x.Id))
                .ToList();

            _unitOfWork.SearchHistories.DeleteRange(oldestSearches);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}