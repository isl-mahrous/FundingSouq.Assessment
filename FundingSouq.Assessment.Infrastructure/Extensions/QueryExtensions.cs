using FundingSouq.Assessment.Core.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Extensions;

public static class QueryExtensions
{
    public static async Task<PagedResult<T>> ToPagedAsync<T>(this IQueryable<T> source, int page = 1,
        int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await source.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<T>(items, page, pageSize, totalCount, totalPages);
    }
}