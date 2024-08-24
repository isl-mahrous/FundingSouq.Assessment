using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

/// <summary>
/// Provides a repository interface for managing <see cref="Client"/> entities with additional client-specific operations.
/// </summary>
public interface IClientRepository : IRepository<Client>
{
    /// <summary>
    /// Retrieves a paged list of clients, including filtering and sorting based on the provided payload.
    /// </summary>
    /// <param name="payload">The paging payload containing search key, sort key, page number, and page size.</param>
    /// <returns>A task that represents the asynchronous operation, containing a paged result of <see cref="ClientDto"/>.</returns>
    Task<PagedResult<ClientDto>> GetClientsAsPagedAsync(PagingPayload payload);
}
