using System.Linq.Expressions;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using FundingSouq.Assessment.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Repositories;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext DbContext;

    public Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().UpdateRange(entities);
    }

    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task<PagedResult<TEntity>> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        int page = 1,
        int pageSize = 10,
        params Expression<Func<TEntity, object>>[] includes
    )
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        if (predicate is not null) query = query.Where(predicate);

        if (orderBy is not null)
        {
            query = sortDirection == SortDirection.Descending
                ? query.OrderByDescending(orderBy).ThenByDescending(x => x.Id)
                : query.OrderBy(orderBy).ThenBy(x => x.Id);
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        return await query.ToPagedAsync(page, pageSize);
    }

    public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.SingleAsync(x => x.Id == id);
    }

    public IQueryable<TEntity> GetAsQueryable(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return DbContext.Set<TEntity>().AsQueryable();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        return await query.AnyAsync();
    }

    public async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        await query.ExecuteDeleteAsync();
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    public void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        return await query.CountAsync();
    }
}