using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Use Table-Per-Hierarchy (TPH) mapping strategy for inheritance
        builder.UseTphMappingStrategy();

        // Set discriminator column for UserType with specific values for derived classes
        builder.HasDiscriminator(x => x.UserType)
            .HasValue<HubUser>(UserType.HubUser)
            .HasValue<Client>(UserType.Client);
        
        // Configure Email property to be case insensitive and use utf8 collation for case-insensitive search
        builder.Property(x => x.Email)
            .HasColumnType("citext")
            .UseCollation("en_US.utf8")
            .IsRequired()
            .HasMaxLength(100); // Email is required and has a max length of 100 characters
        
        // Configure FirstName and LastName properties
        builder.Property(x => x.FirstName)
            .IsRequired()       // FirstName is required
            .HasMaxLength(60);  // Max length of 60 characters

        builder.Property(x => x.LastName)
            .IsRequired()       // LastName is required
            .HasMaxLength(60);  // Max length of 60 characters
        
        // Create indexes to improve query performance
        builder.HasIndex(x => x.Email).IsUnique();  // Unique index on Email
        builder.HasIndex(x => x.UserType);          // Index on UserType for faster lookups
    }
}
