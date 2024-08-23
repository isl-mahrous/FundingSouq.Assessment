using System.Data;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FundingSouq.Assessment.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction _transaction;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // Repositories
    public IRepository<User> Users => GetRepository<User>();
    public IRepository<HubUser> HubUsers => GetRepository<HubUser>();
    public IClientRepository Clients => GetClientRepository();
    public IRepository<Address> Addresses => GetRepository<Address>();
    public IRepository<Account> Accounts => GetRepository<Account>();
    public IRepository<Country> Countries => GetRepository<Country>();
    public IRepository<City> Cities => GetRepository<City>();
    public IRepository<SearchHistory> SearchHistories => GetRepository<SearchHistory>();
    public IRepository<HubPage> HubPages => GetRepository<HubPage>();
    
    private IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
            return (IRepository<TEntity>) _repositories[typeof(TEntity)];

        var repository = new Repository<TEntity>(_dbContext);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }
    
    private IClientRepository GetClientRepository()
    {
        if (_repositories.ContainsKey(typeof(Client)))
            return (IClientRepository) _repositories[typeof(Client)];

        var repository = new ClientRepository(_dbContext);
        _repositories.Add(typeof(Client), repository);
        return repository;
    }
    
    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(isolationLevel);
    }    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken: cancellationToken);
    }

    public async Task CommitTransactionAsync()
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.CommitAsync();
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync()
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.RollbackAsync();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if(_transaction is null)
            throw new InvalidOperationException("Transaction has not been started");
        
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _dbContext.SaveChangesAsync(token);
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_dbContext != null) await _dbContext.DisposeAsync();
        if (_transaction != null) await _transaction.DisposeAsync();
    }

    public void Dispose()
    {
        if (_dbContext != null) _dbContext.Dispose();
        if (_transaction != null) _transaction.Dispose();
    }
}