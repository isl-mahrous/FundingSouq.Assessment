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

/// <summary>
/// Provides an implementation of the <see cref="IClientRepository"/> interface for managing <see cref="Client"/> entities.
/// </summary>
internal class ClientRepository : Repository<Client>, IClientRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context to be used by the repository.</param>
    public ClientRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<PagedResult<ClientDto>> GetClientsAsPagedAsync(PagingPayload payload)
    {
        var query = DbContext.Clients.AsQueryable();

        // Apply search filtering if a search key is provided
        if (payload.SearchKey.IsNotEmpty())
        {
            var predicate = PredicateBuilder.New<Client>();
            var key = $"%{payload.SearchKey.ToLower()}%";

            // Build the search predicate for filtering by multiple client fields
            predicate = predicate
                .Or(x => EF.Functions.ILike(x.FirstName, key))
                .Or(x => EF.Functions.ILike(x.LastName, key))
                .Or(x => EF.Functions.ILike(x.Email, key))
                .Or(x => EF.Functions.ILike(x.PersonalId, key))
                .Or(x => EF.Functions.ILike(x.MobileNumber, key))
                .Or(x => x.Accounts.Any(a => EF.Functions.ILike(a.AccountNumber, key)));

            // Additional filtering by ID if the search key can be parsed as an integer
            if (int.TryParse(key, out var intKey) && intKey > 0)
            {
                predicate = predicate.Or(x => x.Id == intKey);
            }

            query = query.Where(predicate);
        }

        // Apply sorting based on the specified sort key and direction
        var sortKey = GetClientSortKey(payload.SortKey);
        query = payload.SortDirection == SortDirection.Ascending
            ? query.OrderBy(sortKey)
            : query.OrderByDescending(sortKey);

        // Project the query results into ClientDto objects for the paged result
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

        // Return the paged result of ClientDto objects
        return await projection.ToPagedAsync(payload.Page, payload.PageSize);
    }

    /// <summary>
    /// Determines the sorting key based on the provided sort key string.
    /// </summary>
    /// <param name="sortKey">The sort key as a string.</param>
    /// <returns>An expression that defines the sort key for the query.</returns>
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
