using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundingSouq.Assessment.Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    // DbSets representing the tables in the database
    public DbSet<User> Users { get; set; }
    public DbSet<HubUser> HubUsers { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<HubPage> HubPages { get; set; }
}
