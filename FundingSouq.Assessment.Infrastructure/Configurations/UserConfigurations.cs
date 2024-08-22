using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // setting up table per hierarchy mapping strategy for inheritance
        builder.UseTphMappingStrategy();
        builder.HasDiscriminator(x => x.UserType)
            .HasValue<HubUser>(UserType.HubUser)
            .HasValue<Client>(UserType.Client);
        
        // Setting up case insensitive email column with utf8 collation to support case insensitive search
        builder.Property(x => x.Email)
            .HasColumnType("citext")
            .UseCollation("en_US.utf8");
        
        // Setting properties
        builder.Property(x=>x.Email).IsRequired().HasMaxLength(100);
        builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(60);
        builder.Property(x=>x.LastName).IsRequired().HasMaxLength(60);
        
        // Indexes to improve performance
        builder.HasIndex(x=>x.Email).IsUnique();
        builder.HasIndex(x=>x.UserType);
        
    }
}