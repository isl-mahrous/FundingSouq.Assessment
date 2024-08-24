using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class AccountConfigurations : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Set the primary key
        builder.HasKey(x => x.Id);
        
        // Set property configurations
        builder.Property(x => x.AccountNumber)
            .IsRequired()       // Account number is required
            .HasMaxLength(25);  // Maximum length of 25 characters
        
        // Configure relationships
        builder.HasOne(x => x.Client)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.ClientId); // Set foreign key for Client
        
        // Create indexes to improve query performance
        builder.HasIndex(x => x.ClientId);           // Index on ClientId for faster lookups
        builder.HasIndex(x => x.AccountNumber)       // Unique index on AccountNumber to enforce uniqueness
            .IsUnique();
    }
}