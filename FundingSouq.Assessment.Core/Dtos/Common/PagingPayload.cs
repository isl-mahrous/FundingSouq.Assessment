using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents the payload for paging, searching, and sorting operations.
/// </summary>
/// <remarks>
/// This class is used to encapsulate the parameters needed for paginated queries, 
/// including search criteria, pagination settings, and sorting options.
/// </remarks>
public class PagingPayload
{
    /// <summary>
    /// Gets or sets the search key used for filtering results.
    /// </summary>
    /// <remarks>
    /// The search key is used to filter the data based on a specific term. If no search key is provided, 
    /// all records will be returned based on the pagination settings.
    /// </remarks>
    public string SearchKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current page number for pagination.
    /// </summary>
    /// <remarks>
    /// The page number indicates which page of results to retrieve. The default value is 1.
    /// </remarks>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <remarks>
    /// The page size determines how many items are returned in each page of results. The default value is 10.
    /// </remarks>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the key used to sort the results.
    /// </summary>
    /// <remarks>
    /// The sort key determines the field by which the results are sorted. If no sort key is provided, 
    /// results will be sorted by the default field.
    /// </remarks>
    public string SortKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the direction in which to sort the results.
    /// </summary>
    /// <remarks>
    /// The sort direction indicates whether the results should be sorted in ascending or descending order.
    /// The default value is ascending.
    /// see <see cref="SortDirection"/>
    /// </remarks>
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}
