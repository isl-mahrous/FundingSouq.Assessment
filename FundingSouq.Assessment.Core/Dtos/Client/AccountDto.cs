using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the data transfer object (DTO) for a bank account.
/// </summary>
/// <remarks>
/// This DTO includes details of a bank account, such as the account number, type, 
/// balance, and the associated client.
/// </remarks>
public class AccountDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the account.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the client associated with this account.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the type of the bank account.
    /// see <see cref="BankAccountType"/>
    /// </summary>
    public BankAccountType AccountType { get; set; }

    /// <summary>
    /// Gets the account type as a string.
    /// </summary>
    public string AccountTypeStr => AccountType.ToString();

    /// <summary>
    /// Gets or sets the current balance of the account.
    /// </summary>
    public decimal Balance { get; set; }
}
