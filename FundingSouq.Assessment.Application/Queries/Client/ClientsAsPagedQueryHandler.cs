using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Handler for processing <see cref="ClientsAsPagedQuery"/>.
/// </summary>
/// <remarks>
/// This handler retrieves a paged list of clients from the database with optional filtering and sorting.
/// It also saves the search history for the user if the request was successful.
/// </remarks>
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
        // Adapt the query request to a PagingPayload object
        var pagingPayload = request.Adapt<PagingPayload>();

        // Retrieve the paged list of clients based on the paging payload
        var clients = await _unitOfWork.Clients.GetClientsAsPagedAsync(pagingPayload);

        // If the HTTP context is available, save the search history
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

            // Attempt to save the search history and log any errors
            var result = await _sender.Send(saveSearchCommand, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to save search history for user {UserId}. Error: {Message}", request.UserId,
                    result.Error.Message);
            }
        }

        // Return the paged result of clients
        return clients;
    }

    /// <summary>
    /// Constructs a query string based on the provided paging payload.
    /// </summary>
    /// <param name="payload">The paging payload used to build the query string.</param>
    /// <returns>A string representing the query parameters.</returns>
    private string GetSearchQuery(PagingPayload payload)
    {
        return $"page={payload.Page}&pageSize={payload.PageSize}&sortKey={payload.SortKey}" +
               $"&sortDirection={(int)payload.SortDirection}&searchKey={payload.SearchKey}";
    }
}