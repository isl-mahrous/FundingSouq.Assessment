using System.Data;
using FundingSouq.Assessment.Core.Entities;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken token);
    IRepository<User> Users { get; }
    IRepository<HubUser> HubUsers { get; }
    IClientRepository Clients { get; }
    IRepository<Address> Addresses { get; }
    IRepository<Account> Accounts { get; }
    IRepository<Country> Countries { get; }
    IRepository<City> Cities { get; }
    IRepository<SearchHistory> SearchHistories { get; }
    IRepository<HubPage> HubPages { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}