namespace FundingSouq.Assessment.Core.Entities;

public class HubPage : BaseEntity
{
    public string Key { get; set; }
    public virtual ICollection<SearchHistory> SearchHistory { get; set; } = new List<SearchHistory>();
}