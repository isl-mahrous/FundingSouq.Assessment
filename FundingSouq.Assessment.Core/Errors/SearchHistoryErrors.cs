using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

/// <summary>
/// Provides a collection of predefined error messages related to search history operations.
/// </summary>
public static class SearchHistoryErrors
{
    /// <summary>
    /// Error indicating that the page key was not found.
    /// </summary>
    public static readonly Error PageKeyNotFound = new Error("PAGE_KEY_NOT_FOUND", "Page key not found.");
}