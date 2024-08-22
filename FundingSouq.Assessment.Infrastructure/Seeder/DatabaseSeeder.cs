using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Seeder;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task Seed()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Seed Countries and Cities
            await SeedCountriesAndCities();

            // Seed Hub Page
            await SeedHubPage();

            // Seed Super Admin User
            await SeedSuperAdmin();

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task SeedCountriesAndCities()
    {
        if (await _context.Countries.AnyAsync()) return;

        var saudiArabia = new Country { Id = 1, Name = "Saudi Arabia", Code = "SA", PhonePrefix = "+966" };
        var uae = new Country { Id = 2, Name = "United Arab Emirates", Code = "AE", PhonePrefix = "+971" };
        var egypt = new Country { Id = 3, Name = "Egypt", Code = "EG", PhonePrefix = "+20" };

        _context.Countries.AddRange(saudiArabia, uae, egypt);
        await _context.SaveChangesAsync();

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

        _context.Cities.AddRange(saCities);
        _context.Cities.AddRange(uaeCities);
        _context.Cities.AddRange(egyptCities);

        await _context.SaveChangesAsync();
    }

    private async Task SeedHubPage()
    {
        if (await _context.HubPages.AnyAsync()) return;

        var clientsSearchPage = new HubPage
        {
            Key = "clients-search"
        };
        _context.HubPages.Add(clientsSearchPage);
        await _context.SaveChangesAsync();
    }

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
}