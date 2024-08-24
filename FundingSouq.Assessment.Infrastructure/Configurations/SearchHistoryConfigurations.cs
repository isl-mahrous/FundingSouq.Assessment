using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class SearchHistoryConfigurations : IEntityTypeConfiguration<SearchHistory>
{
    public void Configure(EntityTypeBuilder<SearchHistory> builder)
    {
        builder.HasKey(x => x.Id); // Setting the primary key
        
        // Setting properties
        builder.Property(x => x.SearchQuery)
            .IsRequired(); // SearchQuery is required
        
        // Setting up relationships
        builder.HasOne(x => x.HubUser)
            .WithMany(x => x.SearchHistory)
            .HasForeignKey(x => x.HubUserId); // One-to-many relationship with HubUser
        
        builder.HasOne(x => x.HubPage)
            .WithMany(x => x.SearchHistory)
            .HasForeignKey(x => x.HubPageId); // One-to-many relationship with HubPage
        
        // Indexes to improve performance
        builder.HasIndex(x => x.HubUserId); // Index on HubUserId for faster lookups
        builder.HasIndex(x => x.HubPageId); // Index on HubPageId for faster lookups
    }
}
