using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

public class DeleteClientCommand : IRequest<Result>
{
    public int ClientId { get; set; }
}

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId, c => c.Accounts, c => c.Addresses);
        if (client == null) return ClientErrors.ClientNotFound;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        _unitOfWork.Clients.Delete(client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return Result.Success();
    }
}