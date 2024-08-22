using System.Data;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken token); 
}