using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class AccountConfigurations : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Setting properties
        builder.Property(x=>x.AccountNumber).IsRequired().HasMaxLength(25);
        
        // setting up relations
        builder.HasOne(x=>x.Client).WithMany(x=>x.Accounts).HasForeignKey(x=>x.ClientId);
        
        // indexes to improve performance
        builder.HasIndex(x=>x.ClientId);
        builder.HasIndex(x=>x.AccountNumber).IsUnique();
    }
}