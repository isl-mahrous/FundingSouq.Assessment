using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a bank account associated with a client in the application.
/// </summary>
/// <remarks>
/// This class contains details about a bank account, including the account number, account type, 
/// balance, and the associated client.
/// </remarks>
public class Account : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the client to whom this account belongs.
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
    /// Gets or sets the current balance of the account.
    /// </summary>
    public decimal Balance { get; set; }
    
    /// <summary>
    /// Navigation property for the client associated with this account.
    /// </summary>
    public virtual Client Client { get; set; }
}
