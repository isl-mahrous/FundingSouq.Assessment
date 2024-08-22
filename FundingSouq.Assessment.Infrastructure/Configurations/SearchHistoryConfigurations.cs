using FundingSouq.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundingSouq.Assessment.Infrastructure.Configurations;

public class SearchHistoryConfigurations : IEntityTypeConfiguration<SearchHistory>
{
    public void Configure(EntityTypeBuilder<SearchHistory> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Setting properties
        builder.Property(x => x.SearchQuery).IsRequired();
        
        // setting up relation
        builder.HasOne(x=>x.HubUser).WithMany(x=>x.SearchHistory).HasForeignKey(x=>x.HubUserId);
        builder.HasOne(x=>x.HubPage).WithMany(x=>x.SearchHistory).HasForeignKey(x=>x.HubPageId);
        
        // indexes to improve performance
        builder.HasIndex(x=>x.HubUserId);
        builder.HasIndex(x=>x.HubPageId);
    }
}