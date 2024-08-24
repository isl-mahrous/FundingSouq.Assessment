using System.Data;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FundingSouq.Assessment.Infrastructure.Repositories;

/// <summary>
/// Provides an implementation of the unit of work pattern, coordinating the work of multiple repositories and managing transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction _transaction;
    private readonly Dictionary<Type, object> _repositories = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">The database context to be used by the unit of work.</param>
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // Repository properties
    public IRepository<User> Users => GetRepository<User>();
    public IRepository<HubUser> HubUsers => GetRepository<HubUser>();
    public IClientRepository Clients => GetClientRepository();
    public IRepository<Address> Addresses => GetRepository<Address>();
    public IRepository<Account> Accounts => GetRepository<Account>();
    public IRepository<Country> Countries => GetRepository<Country>();
    public IRepository<City> Cities => GetRepository<City>();
    public IRepository<SearchHistory> SearchHistories => GetRepository<SearchHistory>();
    public IRepository<HubPage> HubPages => GetRepository<HubPage>();

    /// <summary>
    /// Retrieves a repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The repository instance.</returns>
    private IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
            return (IRepository<TEntity>) _repositories[typeof(TEntity)];

        var repository = new Repository<TEntity>(_dbContext);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }
    
    /// <summary>
    /// Retrieves the repository for the <see cref="Client"/> entity.
    /// </summary>
    /// <returns>The client repository instance.</returns>
    private IClientRepository GetClientRepository()
    {
        if (_repositories.ContainsKey(typeof(Client)))
            return (IClientRepository) _repositories[typeof(Client)];

        var repository = new ClientRepository(_dbContext);
        _repositories.Add(typeof(Client), repository);
        return repository;
    }
    
    /// <inheritdoc />
    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(isolationLevel);
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync()
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.CommitAsync();
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.CommitAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync()
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.RollbackAsync();
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.RollbackAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _dbContext.SaveChangesAsync(token);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_dbContext != null) await _dbContext.DisposeAsync();
        if (_transaction != null) await _transaction.DisposeAsync();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_dbContext != null) _dbContext.Dispose();
        if (_transaction != null) _transaction.Dispose();
    }
}