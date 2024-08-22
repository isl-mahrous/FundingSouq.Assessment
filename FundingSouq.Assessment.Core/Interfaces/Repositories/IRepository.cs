using System.Linq.Expressions;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// This lets you add an entity locally, without having to hit the database, and then commiting your update
    /// by calling SaveChangesAsync() and CommitTransactionAsync()
    /// </summary>
    /// <param name="entity">The entity object you need to update</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    void Add(TEntity entity);

    /// <summary>
    /// Exposes AddRange method of the DbContext, without hitting the database, similar to Add
    /// </summary>
    /// <param name="entities">List of entities to Add</param>
    /// <typeparam name="TEntity">Entity Type</typeparam>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// This lets you update an entity locally, without having to hit the database, and then commiting your update
    /// by calling SaveChangesAsync() and CommitTransactionAsync()
    /// </summary>
    /// <param name="entity">The entity object you need to update</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    void Update(TEntity entity);

    /// <summary>
    /// Exposes UpdateRange method of the DbContext, without hitting the database, similar to Update
    /// </summary>
    /// <param name="entities">List of entities to Update</param>
    /// <typeparam name="TEntity">Entity Type</typeparam>
    void UpdateRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Get any TEntity based on the predicate provided and includes if any
    /// </summary>
    /// <typeparam name="TEntity">The entity object you need to fetch</typeparam>
    /// <returns>Entity type</returns>
    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Get all TEntity matching the predicate provided and includes if any.
    /// </summary>
    /// <typeparam name="TEntity">The entity object you need to fetch</typeparam>
    /// <returns>Entity type</returns>
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Get all TEntity matching the predicate provided in a paged format.
    /// </summary>
    /// <param name="predicate">the predicate to filter the entities</param> 
    /// <param name="orderBy">the order by expression</param>
    /// <param name="sortDirection">the sort direction</param>
    /// <param name="page">the page number</param>
    /// <param name="pageSize">the page size</param>
    /// <param name="includes"></param>
    /// <returns name="PagedResult">Paged result of TEntity</returns>
    Task<PagedResult<TEntity>> GetPagedAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        int page = 1,
        int pageSize = 10,
        params Expression<Func<TEntity, object>>[] includes
        );

    /// <summary>
    ///  Get TEntity by Id and includes if any
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Get any TEntity as a queryable object based on the predicate provided and includes if any
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    IQueryable<TEntity> GetAsQueryable(Expression<Func<TEntity, bool>> predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Check if any entity matching the predicate exists in the database
    /// </summary>
    /// <typeparam name="TEntity">The entity object we need to check existence of</typeparam>
    /// <returns>Entity type</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Runs an execute delete command that matches the specified predicate
    /// </summary>
    /// <typeparam name="TEntity">The entity object we need to check existence of</typeparam>
    /// <returns>Entity type</returns>
    Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate = null);


    /// <summary>
    /// Mark a list of entities as deleted
    /// </summary>
    /// <param name="entities">The entities required for delete</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    void DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Mark a single entity as deleted
    /// </summary>
    /// <param name="entity">The entity required for delete</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    void Delete(TEntity entity);

    /// <summary>
    /// Count the number of entities matching the predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
}