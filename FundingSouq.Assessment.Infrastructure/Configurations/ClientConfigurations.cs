using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class ClientConfigurations : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        // Setting properties with constraints
        builder.Property(x => x.PersonalId)
            .IsRequired()       // PersonalId is required
            .HasMaxLength(11);  // Max length of 11 characters

        builder.Property(x => x.MobileNumber)
            .IsRequired()       // MobileNumber is required
            .HasMaxLength(25);  // Max length of 25 characters
        
        // Setting up relationships
        builder.HasMany(x => x.Addresses)
            .WithOne(x => x.Client)
            .HasForeignKey(x => x.ClientId); // One-to-many relationship with Addresses

        builder.HasMany(x => x.Accounts)
            .WithOne(x => x.Client)
            .HasForeignKey(x => x.ClientId); // One-to-many relationship with Accounts
        
        // Indexes to improve performance
        builder.HasIndex(x => x.PersonalId).IsUnique();   // Unique index on PersonalId
        builder.HasIndex(x => x.MobileNumber).IsUnique(); // Unique index on MobileNumber
    }
}
