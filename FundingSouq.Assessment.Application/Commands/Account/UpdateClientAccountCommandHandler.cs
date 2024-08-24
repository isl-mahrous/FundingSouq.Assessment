using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="UpdateClientAccountCommand"/>.
/// </summary>
/// <remarks>
/// This handler updates the details of a client account if the account exists and the new account number
/// does not conflict with an existing account. If the account is successfully updated, the result is returned 
/// as an <see cref="AccountDto"/> object.
/// </remarks>
public class UpdateClientAccountCommandHandler : IRequestHandler<UpdateClientAccountCommand, Result<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountDto>> Handle(UpdateClientAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetFirstAsync(a => a.Id == request.Id);
        if (account == null) return AccountErrors.AccountNotFound;

        var accountNumberExists = await _unitOfWork.Accounts
            .ExistsAsync(a => a.AccountNumber == request.AccountNumber && a.Id != request.Id);
        if (accountNumberExists) return AccountErrors.AccountNumberExists;

        account.AccountNumber = request.AccountNumber;
        account.AccountType = request.AccountType;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return account.Adapt<AccountDto>();
    }
}