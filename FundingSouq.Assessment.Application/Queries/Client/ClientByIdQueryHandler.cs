using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Handler for processing <see cref="ClientByIdQuery"/>.
/// </summary>
/// <remarks>
/// This handler retrieves a client from the database based on the provided ID.
/// The client data is returned as a <see cref="ClientDto"/> if found; otherwise, an error is returned.
/// </remarks>
public class ClientByIdQueryHandler : IRequestHandler<ClientByIdQuery, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<ClientDto>> Handle(ClientByIdQuery request, CancellationToken cancellationToken)
    {
        // Fetch the client with the specified ID, including related accounts and addresses
        var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId, c => c.Accounts, c => c.Addresses);
        
        // If the client is not found, return an error
        if (client == null) return ClientErrors.ClientNotFound;
        
        // Convert the client entity to ClientDto and return it
        return client.Adapt<ClientDto>();
    }
}