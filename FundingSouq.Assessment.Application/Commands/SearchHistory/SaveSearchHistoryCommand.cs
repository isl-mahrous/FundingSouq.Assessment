using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MapsterMapper;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

public class SaveSearchHistoryCommand : IRequest<Result>
{
    public int HubUserId { get; set; }
    public string HubPageKey { get; set; }
    public string SearchQuery { get; set; }
    public DateTime SearchDate { get; set; }
}

public class SaveSearchHistoryCommandHandler : IRequestHandler<SaveSearchHistoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public SaveSearchHistoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SaveSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        var hubPage = await _unitOfWork.HubPages.GetFirstAsync(x=>x.Key == request.HubPageKey);
        if (hubPage is null)
        {
            hubPage = new HubPage
            {
                Key = request.HubPageKey
            };
            _unitOfWork.HubPages.Add(hubPage);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        
        var existingSearches = await _unitOfWork.SearchHistories
            .GetAllAsync(x => x.HubUserId == request.HubUserId && x.HubPageId == hubPage.Id);

        var existingSearch = existingSearches.FirstOrDefault(x => x.SearchQuery == request.SearchQuery);
        if (existingSearch != null)
        {
            existingSearch.SearchDate = DateTime.UtcNow;
        }
        else
        {
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