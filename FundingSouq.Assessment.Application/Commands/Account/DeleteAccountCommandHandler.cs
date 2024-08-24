using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="DeleteClientAccountCommand"/>.
/// </summary>
/// <remarks>
/// This handler deletes a client account after ensuring that the client has more than one account.
/// If the client has only one account, the deletion is not allowed, and an error is returned.
/// </remarks>
public class DeleteAccountCommandHandler : IRequestHandler<DeleteClientAccountCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteClientAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetFirstAsync(a => a.Id == request.Id);
        if (account == null) return AccountErrors.AccountNotFound;
        
        var clientAccountsCount = await _unitOfWork.Accounts.CountAsync(a => a.ClientId == account.ClientId);
        if (clientAccountsCount == 1) return AccountErrors.ClientHasOneAccount;
        
        _unitOfWork.Accounts.Delete(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}