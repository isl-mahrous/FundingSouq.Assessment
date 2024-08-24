using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

public class AccountByIdQuery : IRequest<Result<AccountDto>>
{
    public int Id { get; set; }
}

public class AccountByIdQueryHandler : IRequestHandler<AccountByIdQuery, Result<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountDto>> Handle(AccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetFirstAsync(a => a.Id == request.Id);
        if (account == null) return AccountErrors.AccountNotFound;

        return account.Adapt<AccountDto>();
    }
}