using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class CountryConfigurations : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Setting properties
        builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x=>x.Code).IsRequired().HasMaxLength(5);
        builder.Property(x=>x.PhonePrefix).IsRequired().HasMaxLength(5);
        
        // setting up relations
        builder.HasMany(x=>x.Cities).WithOne(x=>x.Country).HasForeignKey(x=>x.CountryId);
        
        // indexes to improve performance
        builder.HasIndex(x=>x.Name).IsUnique();
        builder.HasIndex(x=>x.Code).IsUnique();
    }
}