using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to create a new account for a client.
/// </summary>
/// <remarks>
/// This command returns a <see cref="Result{T}"/> where T is <see cref="AccountDto"/>.
/// </remarks>
public class CreateClientAccountCommand : IRequest<Result<AccountDto>>
{
    /// <summary>
    /// Gets or sets the Client ID.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the type of the bank account.
    /// </summary>
    public BankAccountType AccountType { get; set; }
}