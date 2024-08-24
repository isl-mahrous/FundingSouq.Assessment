using System.Linq.Expressions;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

/// <summary>
/// Defines a generic repository interface for common data access operations.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Adds an entity to the context without saving it to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(TEntity entity);

    /// <summary>
    /// Adds a range of entities to the context without saving them to the database.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates an entity in the context without saving it to the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Updates a range of entities in the context without saving them to the database.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    void UpdateRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Retrieves the first entity matching the specified predicate, including related entities.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>The first entity that matches the predicate.</returns>
    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Retrieves all entities matching the specified predicate, including related entities.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A list of entities that match the predicate.</returns>
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Retrieves entities in a paged format, including related entities.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities.</param>
    /// <param name="orderBy">The expression to order the entities.</param>
    /// <param name="sortDirection">The sort direction (ascending or descending).</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result of entities.</returns>
    Task<PagedResult<TEntity>> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        int page = 1,
        int pageSize = 10,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Retrieves an entity by its ID, including related entities.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>The entity with the specified ID.</returns>
    Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Returns a queryable object of entities matching the specified predicate, including related entities.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A queryable object of entities.</returns>
    IQueryable<TEntity> GetAsQueryable(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Checks if any entities matching the specified predicate exist in the database.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities.</param>
    /// <returns><c>true</c> if any entities exist that match the predicate; otherwise, <c>false</c>.</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Executes a delete command on entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities for deletion.</param>
    Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Marks a range of entities as deleted in the context.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    void DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Marks an entity as deleted in the context.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Counts the number of entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to filter the entities.</param>
    /// <returns>The count of entities that match the predicate.</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
}
