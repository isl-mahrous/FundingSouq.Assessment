namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents a paged result set with metadata for pagination.
/// </summary>
/// <typeparam name="T">The type of data contained in the result set.</typeparam>
/// <remarks>
/// This class encapsulates the result of a paginated query, including the data items, 
/// the current page, page size, total item count, and total number of pages.
/// </remarks>
public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the data items for the current page.
    /// </summary>
    public IEnumerable<T> Data { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the size of the page (number of items per page).
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class with specified data and pagination metadata.
    /// </summary>
    /// <param name="data">The data items for the current page.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The size of the page (number of items per page).</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="totalPages">The total number of pages.</param>
    public PagedResult(IEnumerable<T> data, int page, int pageSize, int totalCount, int totalPages)
    {
        Data = data;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}
