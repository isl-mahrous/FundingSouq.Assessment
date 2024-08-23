using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

public class UserSearchHistoryQuery : IRequest<Result<List<SearchHistoryDto>>>
{
    public int HubUserId { get; set; }
    public string HubPageKey { get; set; }
}

public class UserSearchHistoryQueryHandler : IRequestHandler<UserSearchHistoryQuery, Result<List<SearchHistoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserSearchHistoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SearchHistoryDto>>> Handle(UserSearchHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var searchHistoryList = await _unitOfWork.SearchHistories.GetAllAsync(
            s => s.HubUserId == request.HubUserId &&
                 (string.IsNullOrEmpty(request.HubPageKey) || s.HubPage.Key == request.HubPageKey),
            includes: s => s.HubPage);

        var result = searchHistoryList
            .GroupBy(s => s.HubPage.Key)
            .Select(history => new SearchHistoryDto
            {
                HubPageId = history.First().HubPage.Id,
                HubPageKey = history.Key,
                History = history
                    .OrderByDescending(s => s.SearchDate)
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