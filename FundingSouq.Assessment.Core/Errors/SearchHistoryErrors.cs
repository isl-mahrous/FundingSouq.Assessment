using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Errors;

public static class SearchHistoryErrors
{
    public static readonly Error PageKeyNotFound = new Error("PAGE_KEY_NOT_FOUND", "Page key not found.");
}