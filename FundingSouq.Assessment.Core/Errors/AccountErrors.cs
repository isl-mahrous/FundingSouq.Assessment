using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

public static class AccountErrors
{
    public static readonly Error AccountNotFound = new Error("ACCOUNT_NOT_FOUND", "Account not found.");
    public static readonly Error AccountNumberExists = new Error("ACCOUNT_NUMBER_EXISTS", "Account number already exists.");
    public static readonly Error ClientHasOneAccount = new Error("CLIENT_HAS_ONE_ACCOUNT", "Client has only one account. It cannot be deleted.");
}