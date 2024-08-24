using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to update a client's account details.
/// </summary>
/// <remarks>
/// This command is used to update the account number and account type of a specific client account.
/// The result of the operation is encapsulated in a <see cref="Result{T}"/> object, where T is <see cref="AccountDto"/>.
/// </remarks>
public class UpdateClientAccountCommand : IRequest<Result<AccountDto>>
{
    /// <summary>
    /// Gets or sets the Id of the account to be updated.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new account number.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the new account type.
    /// </summary>
    public BankAccountType AccountType { get; set; }
}