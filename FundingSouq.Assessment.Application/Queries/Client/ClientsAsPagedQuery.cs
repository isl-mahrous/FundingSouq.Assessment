using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

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

    public ClientsAsPagedQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<PagedResult<ClientDto>>> Handle(ClientsAsPagedQuery request,
        CancellationToken cancellationToken)
    {
        var pagingPayload = request.Adapt<PagingPayload>();
        var clients = await _unitOfWork.Clients.GetClientsAsPagedAsync(pagingPayload);
        return clients;
    }
}