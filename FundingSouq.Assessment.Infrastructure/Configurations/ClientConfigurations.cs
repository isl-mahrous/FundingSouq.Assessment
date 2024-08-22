using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class ClientConfigurations : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        
        // Setting properties
        builder.Property(x=>x.PersonalId).IsRequired().HasMaxLength(11);
        builder.Property(x=>x.MobileNumber).IsRequired().HasMaxLength(25);
        
        // setting up relations
        builder.HasMany(x=>x.Addresses).WithOne(x=>x.Client).HasForeignKey(x=>x.ClientId);
        builder.HasMany(x=>x.Accounts).WithOne(x=>x.Client).HasForeignKey(x=>x.ClientId);
        
        // indexes to improve performance
        builder.HasIndex(x=>x.PersonalId).IsUnique();
        builder.HasIndex(x=>x.MobileNumber).IsUnique();
    }
}