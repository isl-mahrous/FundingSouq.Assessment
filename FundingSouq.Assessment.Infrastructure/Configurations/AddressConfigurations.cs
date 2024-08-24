using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class AddressConfigurations : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        // Set the primary key
        builder.HasKey(x => x.Id);
        
        // Set property configurations
        builder.Property(x => x.Street)
            .IsRequired()       // Street is required
            .HasMaxLength(100); // Maximum length of 100 characters
        
        builder.Property(x => x.ZipCode)
            .IsRequired()       // ZipCode is required
            .HasMaxLength(10);  // Maximum length of 10 characters

        // Configure relationships
        builder.HasOne(x => x.Client)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.ClientId); // Set foreign key for Client

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId); // Set foreign key for Country

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId); // Set foreign key for City
        
        // Create indexes to improve query performance
        builder.HasIndex(x => x.ClientId);   // Index on ClientId for faster lookups
        builder.HasIndex(x => x.CountryId);  // Index on CountryId for faster lookups
        builder.HasIndex(x => x.CityId);     // Index on CityId for faster lookups
    }
}
