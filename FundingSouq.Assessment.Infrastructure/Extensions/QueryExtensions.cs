using FundingSouq.Assessment.Core.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for querying and projecting the data.
/// </summary>
public static class QueryExtensions
{
    /// <summary>
    /// Asynchronously paginates the source query and returns a <see cref="PagedResult{T}"/> containing the results.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source query.</typeparam>
    /// <param name="source">The queryable collection to paginate.</param>
    /// <param name="page">The current page number (1-based). If less than 1, defaults to 1.</param>
    /// <param name="pageSize">The number of items per page. If less than 1, defaults to 10.</param>
    /// <returns>
    /// A <see cref="PagedResult{T}"/> containing the paginated items, along with pagination metadata such as total count and total pages.
    /// </returns>
    /// <example>
    /// var pagedResult = await myQueryableCollection.ToPagedAsync(page: 2, pageSize: 20);
    /// </example>
    public static async Task<PagedResult<T>> ToPagedAsync<T>(this IQueryable<T> source, int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1; // Ensure page number is at least 1
        if (pageSize < 1) pageSize = 10; // Ensure page size is at least 10

        var totalCount = await source.CountAsync(); // Get the total number of items
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize); // Calculate total pages based on the page size
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); // Get the items for the current page

        // Return the paginated result
        return new PagedResult<T>(items, page, pageSize, totalCount, totalPages);
    }
}
