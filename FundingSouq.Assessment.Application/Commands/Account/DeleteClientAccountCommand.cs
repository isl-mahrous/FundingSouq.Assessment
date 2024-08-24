using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

public class DeleteClientAccountCommand : IRequest<Result>
{
    public int Id { get; set; }
}

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