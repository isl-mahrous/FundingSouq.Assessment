namespace FundingSouq.Assessment.Core.Entities;

public class SearchHistory : BaseEntity
{
    public int HubUserId { get; set; }
    public int HubPageId { get; set; }
    public string SearchQuery { get; set; }
    public DateTime SearchDate { get; set; }
    
    public virtual HubUser HubUser { get; set; }
    public virtual HubPage HubPage { get; set; }
}