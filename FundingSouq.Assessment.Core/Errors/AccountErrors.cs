using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

/// <summary>
/// Provides a collection of predefined error messages related to account operations.
/// </summary>
public static class AccountErrors
{
    /// <summary>
    /// Error indicating that the account was not found.
    /// </summary>
    public static readonly Error AccountNotFound = new Error("ACCOUNT_NOT_FOUND", "Account not found.");

    /// <summary>
    /// Error indicating that the account number already exists.
    /// </summary>
    public static readonly Error AccountNumberExists = new Error("ACCOUNT_NUMBER_EXISTS", "Account number already exists.");

    /// <summary>
    /// Error indicating that the client has only one account, which cannot be deleted.
    /// </summary>
    public static readonly Error ClientHasOneAccount = new Error("CLIENT_HAS_ONE_ACCOUNT", "Client has only one account. It cannot be deleted.");
}
