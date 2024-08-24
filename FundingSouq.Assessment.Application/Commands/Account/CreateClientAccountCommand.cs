using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

public class CreateClientAccountCommand: IRequest<Result<AccountDto>>
{
    public int ClientId { get; set; }
    public string AccountNumber { get; set; }
    public BankAccountType AccountType { get; set; }
}

public class CreateClientAccountCommandHandler : IRequestHandler<CreateClientAccountCommand, Result<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountDto>> Handle(CreateClientAccountCommand request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetFirstAsync(c => c.Id == request.ClientId);
        if (client == null) return ClientErrors.ClientNotFound;
        
        var accountNumberExists = await _unitOfWork.Accounts.ExistsAsync(a => a.AccountNumber == request.AccountNumber);
        if (accountNumberExists) return AccountErrors.AccountNumberExists;
        
        var account = new Account
        {
            AccountNumber = request.AccountNumber,
            AccountType = request.AccountType,
            ClientId = request.ClientId
        };

        _unitOfWork.Accounts.Add(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return account.Adapt<AccountDto>();
    }
}