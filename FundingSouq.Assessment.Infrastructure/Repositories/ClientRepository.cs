using System.Linq.Expressions;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Extensions;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using FundingSouq.Assessment.Infrastructure.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;


namespace FundingSouq.Assessment.Infrastructure.Repositories;
internal class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<PagedResult<ClientDto>> GetClientsAsPagedAsync(PagingPayload payload)
    {
        var query = DbContext.Clients.AsQueryable();

        if (payload.SearchKey.IsNotEmpty())
        {
            var predicate = PredicateBuilder.New<Client>();
            var key =  $"%{payload.SearchKey.ToLower()}%";

            predicate = predicate
                .Or(x => EF.Functions.ILike(x.FirstName, key))
                .Or(x => EF.Functions.ILike(x.LastName, key))
                .Or(x => EF.Functions.ILike(x.Email, key))
                .Or(x => EF.Functions.ILike(x.PersonalId, key))
                .Or(x => EF.Functions.ILike(x.MobileNumber, key))
                .Or(x => x.Accounts.Any(a => EF.Functions.ILike(a.AccountNumber, key)));

            if (int.TryParse(key, out var intKey) && intKey > 0)
            {
                predicate = predicate.Or(x => x.Id == intKey);
            }

            query = query.Where(predicate);
        }
        
        var sortKey = GetClientSortKey(payload.SortKey);
        query = payload.SortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKey)
            : query.OrderByDescending(sortKey);
        
        var projection = query
            .Select(client => new ClientDto
            {
                Id = client.Id,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PersonalId = client.PersonalId,
                ProfilePhoto = client.ProfilePhoto,
                MobileNumber = client.MobileNumber,
                Gender = client.Gender,
                Accounts = client.Accounts
                    .Select(account => new AccountDto
                    {
                        Id = account.Id,
                        AccountNumber = account.AccountNumber,
                        AccountType = account.AccountType,
                        Balance = account.Balance,
                    }).ToList(),
                Addresses = client.Addresses
                    .Select(address => new AddressDto
                    {
                        Id = address.Id,
                        ClientId = address.ClientId,
                        CountryId = address.CountryId,
                        CountryName = address.Country.Name,
                        CityId = address.CityId,
                        CityName = address.City.Name,
                        Street = address.Street,
                        ZipCode = address.ZipCode,
                    }).ToList(),
            });


        return await projection.ToPagedAsync(payload.Page, payload.PageSize);
    }
    
    private Expression<Func<Client, object>> GetClientSortKey(string sortKey)
    {
        return sortKey switch
        {
            "email" => x => x.Email,
            "firstName" => x => x.FirstName,
            "lastName" => x => x.LastName,
            "personalId" => x => x.PersonalId,
            "mobileNumber" => x => x.MobileNumber,
            _ => x => x.Id
        };
    }
}