using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class HubPageConfigurations : IEntityTypeConfiguration<HubPage>
{
    public void Configure(EntityTypeBuilder<HubPage> builder)
    {
        builder.HasKey(x => x.Id);

        // Setting properties
        builder.Property(x => x.Key).IsRequired().HasMaxLength(100);

        // setting up relation
        builder.HasMany(x => x.SearchHistory).WithOne(x => x.HubPage).HasForeignKey(x => x.HubPageId);

        // indexes to improve performance
        builder.HasIndex(x => x.Key).IsUnique();
    }
}