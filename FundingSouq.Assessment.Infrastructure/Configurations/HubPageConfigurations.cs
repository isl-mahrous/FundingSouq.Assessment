using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class HubPageConfigurations : IEntityTypeConfiguration<HubPage>
{
    public void Configure(EntityTypeBuilder<HubPage> builder)
    {
        builder.HasKey(x => x.Id); // Setting the primary key

        // Setting properties with constraints
        builder.Property(x => x.Key)
            .IsRequired()         // Key is required
            .HasMaxLength(100);   // Max length of 100 characters

        // Setting up relationships
        builder.HasMany(x => x.SearchHistory)
            .WithOne(x => x.HubPage)
            .HasForeignKey(x => x.HubPageId); // One-to-many relationship with SearchHistory

        // Indexes to improve performance
        builder.HasIndex(x => x.Key).IsUnique(); // Unique index on Key
    }
}
