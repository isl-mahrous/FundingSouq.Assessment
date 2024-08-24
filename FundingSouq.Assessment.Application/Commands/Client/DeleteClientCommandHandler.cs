using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="DeleteClientCommand"/>.
/// </summary>
/// <remarks>
/// This handler deletes the specified client, including their related accounts and addresses, 
/// by starting a transaction and ensuring all changes are committed or rolled back in case of failure.
/// </remarks>
public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        // Fetch the client with related accounts and addresses
        var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId, c => c.Accounts, c => c.Addresses);
        if (client == null) return ClientErrors.ClientNotFound;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        // Mark the client entity for deletion
        _unitOfWork.Clients.Delete(client);

        // Save changes and commit the transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return Result.Success();
    }
}