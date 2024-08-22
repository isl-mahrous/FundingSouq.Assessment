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
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
    }

    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

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
        var query = _dbContext.Set<TEntity>().AsQueryable();

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
        var query = _dbContext.Set<TEntity>().AsQueryable();

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
        var query = _dbContext.Set<TEntity>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.SingleAsync(x => x.Id == id);
    }

    public IQueryable<TEntity> GetAsQueryable(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return _dbContext.Set<TEntity>().AsQueryable();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        return await query.AnyAsync();
    }

    public async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        await query.ExecuteDeleteAsync();
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null) query = query.Where(predicate);

        return await query.CountAsync();
    }
}