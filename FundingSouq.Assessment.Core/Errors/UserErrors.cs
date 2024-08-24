using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

/// <summary>
/// Provides a collection of predefined error messages related to user operations.
/// </summary>
public static class UserErrors
{
    /// <summary>
    /// Error indicating that the user was not found.
    /// </summary>
    public static readonly Error UserNotFound = new Error("USER_NOT_FOUND", "User not found.");

    /// <summary>
    /// Error indicating that the provided password is invalid.
    /// </summary>
    public static readonly Error InvalidPassword = new Error("INVALID_PASSWORD", "Invalid password.");

    /// <summary>
    /// Error indicating that the provided email is invalid.
    /// </summary>
    public static readonly Error InvalidEmail = new Error("INVALID_EMAIL", "Invalid email.");

    /// <summary>
    /// Error indicating that the email is already in use by another user.
    /// </summary>
    public static readonly Error EmailInUse = new Error("EMAIL_IN_USE", "Email is already in use.");

    /// <summary>
    /// Error indicating that the provided token is invalid.
    /// </summary>
    public static readonly Error InvalidToken = new Error("INVALID_TOKEN", "Invalid token.");

    /// <summary>
    /// Error indicating that the system failed to acquire a lock, and suggests trying again later.
    /// </summary>
    public static readonly Error FailedToAcquireLock =
        new Error("ACQUIRING_LOCK_FAILED", "Failed to acquire lock. Please try again later.");
}
