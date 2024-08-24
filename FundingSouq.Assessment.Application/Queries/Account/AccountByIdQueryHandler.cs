using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Handler for processing <see cref="AccountByIdQuery"/>.
/// </summary>
/// <remarks>
/// This handler retrieves an account from the database based on the provided ID.
/// If the account is found, it is returned as a <see cref="AccountDto"/>; otherwise, an error is returned.
/// </remarks>
public class AccountByIdQueryHandler : IRequestHandler<AccountByIdQuery, Result<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountDto>> Handle(AccountByIdQuery request, CancellationToken cancellationToken)
    {
        // Fetch the account with the specified ID
        var account = await _unitOfWork.Accounts.GetFirstAsync(a => a.Id == request.Id);
        
        // If the account is not found, return an error
        if (account == null) return AccountErrors.AccountNotFound;

        // Convert the account entity to AccountDto and return it
        return account.Adapt<AccountDto>();
    }
}