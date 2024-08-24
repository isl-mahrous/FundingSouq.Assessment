using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handles the <see cref="CreateClientAccountCommand"/> to create a new account for a client.
/// </summary>
public class CreateClientAccountCommandHandler : IRequestHandler<CreateClientAccountCommand, Result<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateClientAccountCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work used to interact with the database.</param>
    public CreateClientAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountDto>> Handle(CreateClientAccountCommand request, CancellationToken cancellationToken)
    {
        // Validate if the client exists
        var client = await _unitOfWork.Clients.GetFirstAsync(c => c.Id == request.ClientId);
        if (client == null) return ClientErrors.ClientNotFound;
        
        // Validate if the account number already exists
        var accountNumberExists = await _unitOfWork.Accounts.ExistsAsync(a => a.AccountNumber == request.AccountNumber);
        if (accountNumberExists) return AccountErrors.AccountNumberExists;
        
        // Create the new account
        var account = new Account
        {
            AccountNumber = request.AccountNumber,
            AccountType = request.AccountType,
            ClientId = request.ClientId
        };

        // Add and save the new account
        _unitOfWork.Accounts.Add(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return the created account
        return account.Adapt<AccountDto>();
    }
}