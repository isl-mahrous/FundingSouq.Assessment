using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class CityConfigurations : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Setting max length for columns and making them required
        builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);
        
        // indexes to improve performance
        builder.HasIndex(x=>x.Name);
        builder.HasIndex(x=>x.CountryId);
    }
}