using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class CityConfigurations : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        // Set the primary key
        builder.HasKey(x => x.Id);
        
        // Set property configurations
        builder.Property(x => x.Name)
            .IsRequired()       // City name is required
            .HasMaxLength(100); // Maximum length of 100 characters
        
        // Create indexes to improve query performance
        builder.HasIndex(x => x.Name);      // Index on Name for faster lookups
        builder.HasIndex(x => x.CountryId); // Index on CountryId for faster lookups
    }
}