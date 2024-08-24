using System.Linq.Expressions;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using FundingSouq.Assessment.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Repositories;

/// <summary>
/// Provides a generic repository implementation for performing data access operations on entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
internal class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext DbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context to use for data access operations.</param>
    public Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    /// <inheritdoc />
    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    /// <inheritdoc />
    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().AddRange(entities);
    }

    /// <inheritdoc />
    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    /// <inheritdoc />
    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().UpdateRange(entities);
    }

    /// <inheritdoc />
    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Include related entities if specified
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Return the first entity that matches the predicate
        return await query.FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Include related entities if specified
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Return a list of all entities that match the predicate
        return await query.ToListAsync();
    }

    /// <inheritdoc />
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

        // Include related entities if specified
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Apply ordering based on the specified orderBy expression and sort direction
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

        // Return a paged result of entities
        return await query.ToPagedAsync(page, pageSize);
    }

    /// <inheritdoc />
    public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Include related entities if specified
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Return the entity with the specified ID
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public IQueryable<TEntity> GetAsQueryable(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Include related entities if specified
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Return the queryable object
        return query;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Return true if any entities match the predicate
        return await query.AnyAsync();
    }

    /// <inheritdoc />
    public async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Execute the delete operation on matching entities
        await query.ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    /// <inheritdoc />
    public void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        // Apply the predicate if provided
        if (predicate is not null) query = query.Where(predicate);

        // Return the count of entities that match the predicate
        return await query.CountAsync();
    }
}
