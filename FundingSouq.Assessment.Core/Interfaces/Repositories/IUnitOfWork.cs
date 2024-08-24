using System.Data;
using FundingSouq.Assessment.Core.Entities;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

/// <summary>
/// Defines the contract for a unit of work, which manages transactions and coordinates the work of multiple repositories.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Begins a transaction with the specified isolation level.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CommitTransactionAsync();
    
    /// <summary>
    /// Begins a transaction with the specified isolation level and cancellation token.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    /// <summary>
    /// Commits the current transaction with the specified cancellation token.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Rolls back the current transaction with the specified cancellation token.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RollbackTransactionAsync();

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveChangesAsync(CancellationToken token);

    // Repository properties
    IRepository<User> Users { get; }
    IRepository<HubUser> HubUsers { get; }
    IClientRepository Clients { get; }
    IRepository<Address> Addresses { get; }
    IRepository<Account> Accounts { get; }
    IRepository<Country> Countries { get; }
    IRepository<City> Cities { get; }
    IRepository<SearchHistory> SearchHistories { get; }
    IRepository<HubPage> HubPages { get; }
}
