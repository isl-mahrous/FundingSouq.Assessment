using Bogus;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Seeder;

/// <summary>
/// Seeds initial data into the database, including countries, cities, users, hub pages, and clients.
/// </summary>
public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseSeeder"/> class.
    /// </summary>
    /// <param name="context">The database context used for seeding.</param>
    /// <param name="passwordHasher">The password hasher used to hash passwords.</param>
    public DatabaseSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    public async Task Seed()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Seed Countries and Cities
            await SeedCountriesAndCities();

            // Seed Hub Pages
            await SeedHubPage();

            // Seed Super Admin User
            await SeedSuperAdmin();

            // Seed Clients
            await SeedClients();

            // Commit transaction if everything is successful
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            // Rollback transaction if an error occurs
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Seeds countries and their respective cities into the database.
    /// </summary>
    private async Task SeedCountriesAndCities()
    {
        // Check if countries already exist
        if (await _context.Countries.AnyAsync()) return;

        // Add countries
        var saudiArabia = new Country { Id = 1, Name = "Saudi Arabia", Code = "SA", PhonePrefix = "+966" };
        var uae = new Country { Id = 2, Name = "United Arab Emirates", Code = "AE", PhonePrefix = "+971" };
        var egypt = new Country { Id = 3, Name = "Egypt", Code = "EG", PhonePrefix = "+20" };

        _context.Countries.AddRange(saudiArabia, uae, egypt);
        await _context.SaveChangesAsync();

        // Add cities for each country
        var saCities = new List<City>
        {
            new City { Name = "Riyadh", CountryId = saudiArabia.Id },
            new City { Name = "Jeddah", CountryId = saudiArabia.Id },
            new City { Name = "Dammam", CountryId = saudiArabia.Id },
        };

        var uaeCities = new List<City>
        {
            new City { Name = "Dubai", CountryId = uae.Id },
            new City { Name = "Abu Dhabi", CountryId = uae.Id },
            new City { Name = "Sharjah", CountryId = uae.Id },
        };

        var egyptCities = new List<City>
        {
            new City { Name = "Cairo", CountryId = egypt.Id },
            new City { Name = "Alexandria", CountryId = egypt.Id },
            new City { Name = "Giza", CountryId = egypt.Id },
        };

        // Save cities to the database
        _context.Cities.AddRange(saCities);
        _context.Cities.AddRange(uaeCities);
        _context.Cities.AddRange(egyptCities);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds a default hub page into the database.
    /// </summary>
    private async Task SeedHubPage()
    {
        if (await _context.HubPages.AnyAsync()) return;

        var clientsSearchPage = new HubPage
        {
            Key = "/api/clients/paged"
        };
        _context.HubPages.Add(clientsSearchPage);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds a super admin user into the database.
    /// </summary>
    private async Task SeedSuperAdmin()
    {
        if (await _context.Users.AnyAsync(u => u.Email == "admin@fundingsouq.com")) return;

        var passwordHash = _passwordHasher.Hash("admin@123");
        var superAdmin = new HubUser
        {
            Email = "admin@fundingsouq.com",
            FirstName = "Super",
            LastName = "Admin",
            PasswordHash = passwordHash,
            Role = HubUserRole.Admin,
            UserType = UserType.HubUser
        };
        _context.Users.Add(superAdmin);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds random client data into the database using the Bogus library.
    /// </summary>
    private async Task SeedClients()
    {
        if (await _context.Clients.AnyAsync()) return;

        // Setup Bogus to generate random clients
        Randomizer.Seed = new Random(8675309);
        
        var addresses = new Faker<Address>()
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.CityId, _ => 1)
            .RuleFor(a => a.CountryId, _ => 1)
            .RuleFor(a => a.ZipCode, f => f.Address.ZipCode());

        var accounts = new Faker<Account>()
            .RuleFor(a => a.AccountNumber, f => f.Finance.Account())
            .RuleFor(a => a.AccountType, f => f.PickRandom<BankAccountType>())
            .RuleFor(a => a.Balance, f => f.Finance.Amount());

        var clients = new Faker<Client>()
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.MobileNumber, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.PasswordHash, _ => _passwordHasher.Hash("client@123"))
            .RuleFor(c => c.UserType, _ => UserType.Client)
            .RuleFor(c => c.PersonalId, f => f.Random.ReplaceNumbers("###########"))
            .RuleFor(c => c.ProfilePhoto, f => f.Image.PlaceholderUrl(500, 500, "Random Person"))
            .RuleFor(c => c.Gender, f => f.PickRandom<Gender>())
            .RuleFor(c => c.Addresses, _ => addresses.Generate(1))
            .RuleFor(c => c.Accounts, _ => accounts.Generate(1));

        // Add generated clients to the database
        _context.Clients.AddRange(clients.Generate(100));
        await _context.SaveChangesAsync();
    }
}
