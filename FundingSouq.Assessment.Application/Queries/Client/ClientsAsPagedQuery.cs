using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FundingSouq.Assessment.Application.Queries;

public class ClientsAsPagedQuery : IRequest<Result<PagedResult<ClientDto>>>
{
    public int UserId { get; set; }
    public string SearchKey { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortKey { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}

public class ClientsAsPagedQueryHandler : IRequestHandler<ClientsAsPagedQuery, Result<PagedResult<ClientDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ClientsAsPagedQueryHandler> _logger;

    public ClientsAsPagedQueryHandler(IUnitOfWork unitOfWork, ISender sender, IHttpContextAccessor httpContextAccessor,
        ILogger<ClientsAsPagedQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<PagedResult<ClientDto>>> Handle(ClientsAsPagedQuery request,
        CancellationToken cancellationToken)
    {
        var pagingPayload = request.Adapt<PagingPayload>();
        var clients = await _unitOfWork.Clients.GetClientsAsPagedAsync(pagingPayload);

        if (_httpContextAccessor.HttpContext != null)
        {
            var route = _httpContextAccessor.HttpContext.Request.Path.Value;
            var searchQuery = GetSearchQuery(pagingPayload);

            var saveSearchCommand = new SaveSearchHistoryCommand
            {
                HubUserId = request.UserId,
                HubPageKey = route,
                SearchQuery = searchQuery,
                SearchDate = DateTime.UtcNow
            };

            var result = await _sender.Send(saveSearchCommand, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to save search history for user {UserId}. Error: {Message}", request.UserId,
                    result.Error.Message);
            }
        }

        return clients;
    }

    private string GetSearchQuery(PagingPayload payload)
    {
        
        return $"page={payload.Page}&pageSize={payload.PageSize}&sortKey={payload.SortKey}" +
               $"&sortDirection={(int)payload.SortDirection}&searchKey={payload.SearchKey}";
    }
}