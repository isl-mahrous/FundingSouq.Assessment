using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class AddressConfigurations : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Setting properties
        builder.Property(x=>x.Street).IsRequired().HasMaxLength(100);
        builder.Property(x=>x.ZipCode).IsRequired().HasMaxLength(10);

        // setting up relations
        builder.HasOne(x => x.Client).WithMany(x => x.Addresses).HasForeignKey(x => x.ClientId);
        builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
        builder.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);
        
        // indexes to improve performance
        builder.HasIndex(x=>x.ClientId);
        builder.HasIndex(x=>x.CountryId);
        builder.HasIndex(x=>x.CityId);
    }
}