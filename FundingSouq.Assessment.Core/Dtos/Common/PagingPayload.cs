using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos.Common;

public class PagingPayload
{
    public string SearchKey { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortKey { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}