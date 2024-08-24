using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FundingSouq.Assessment.Infrastructure.Interceptors;

/// <summary>
/// Intercepts save changes operations in EF Core to update entity timestamps automatically.
/// </summary>
public class BaseEntityInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts synchronous save operations to update entity timestamps.
    /// </summary>
    /// <param name="eventData">Contextual information about the event.</param>
    /// <param name="result">The result of the operation being intercepted.</param>
    /// <returns>The result of the save operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context != null) UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Intercepts asynchronous save operations to update entity timestamps.
    /// </summary>
    /// <param name="eventData">Contextual information about the event.</param>
    /// <param name="result">The result of the operation being intercepted.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the save operation.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null) UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the CreatedAt and LastModifiedAt timestamps for entities.
    /// </summary>
    /// <param name="context">The DbContext that is being saved.</param>
    private void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        // Loop through all tracked entities of type BaseEntity
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                // Set CreatedAt for new entities
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || HasChangedOwnedEntities(entry))
            {
                // Set LastModifiedAt for new or modified entities
                entry.Entity.LastModifiedAt = DateTime.UtcNow;
            }
        }
    }

    /// <summary>
    /// Checks if any owned entities have been added or modified.
    /// </summary>
    /// <param name="entry">The entity entry being checked.</param>
    /// <returns>True if any owned entities are added or modified, otherwise false.</returns>
    private static bool HasChangedOwnedEntities(EntityEntry entry) =>
        entry.References.Any(r => r.TargetEntry != null &&
                                  r.TargetEntry.Metadata.IsOwned() && 
                                  r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}

