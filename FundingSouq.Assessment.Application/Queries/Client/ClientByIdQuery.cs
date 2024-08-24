using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

public class ClientByIdQuery : IRequest<Result<ClientDto>>
{
    public int ClientId { get; set; }
}

public class ClientByIdQueryHandler : IRequestHandler<ClientByIdQuery, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<ClientDto>> Handle(ClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId, c => c.Accounts, c => c.Addresses);
        if (client == null) return ClientErrors.ClientNotFound;
        
        return client.Adapt<ClientDto>();
    }
}