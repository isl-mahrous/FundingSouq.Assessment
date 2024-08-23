using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;

namespace FundingSouq.Assessment.Core.Interfaces.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<PagedResult<ClientDto>> GetClientsAsPagedAsync(PagingPayload payload);
}