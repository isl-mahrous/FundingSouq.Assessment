using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFound = new Error("USER_NOT_FOUND", "User not found.");
    public static readonly Error InvalidPassword = new Error("INVALID_PASSWORD", "Invalid password.");
    public static readonly Error InvalidEmail = new Error("INVALID_EMAIL", "Invalid email.");
    public static readonly Error EmailInUse = new Error("EMAIL_IN_USE", "Email is already in use.");
    public static readonly Error InvalidToken = new Error("INVALID_TOKEN", "Invalid token.");
    public static readonly Error FailedToAcquireLock = new Error("ACQUIRING_LOCK_FAILED", "Failed to acquire lock. Please try again later.");
}