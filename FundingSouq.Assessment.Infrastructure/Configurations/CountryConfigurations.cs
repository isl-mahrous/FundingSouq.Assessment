using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class CountryConfigurations : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id); // Setting the primary key

        // Setting properties with constraints
        builder.Property(x => x.Name)
            .IsRequired()        // Name is required
            .HasMaxLength(100);  // Max length of 100 characters

        builder.Property(x => x.Code)
            .IsRequired()        // Code is required
            .HasMaxLength(5);    // Max length of 5 characters

        builder.Property(x => x.PhonePrefix)
            .IsRequired()        // PhonePrefix is required
            .HasMaxLength(5);    // Max length of 5 characters

        // Setting up relationships
        builder.HasMany(x => x.Cities)
            .WithOne(x => x.Country)
            .HasForeignKey(x => x.CountryId); // One-to-many relationship with Cities

        // Indexes to improve performance
        builder.HasIndex(x => x.Name).IsUnique(); // Unique index on Name
        builder.HasIndex(x => x.Code).IsUnique(); // Unique index on Code
    }
}
