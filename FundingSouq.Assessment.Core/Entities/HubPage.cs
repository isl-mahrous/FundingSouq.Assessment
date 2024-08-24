namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a hub page within the application.
/// </summary>
/// <remarks>
/// This class contains details about a specific page in the hub, identified by a unique key,
/// and tracks the search history associated with that page.
/// </remarks>
public class HubPage : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique key that identifies the hub page.
    /// The key should map to the route of the api called in that page.
    /// <example>/api/clients/paged</example>
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Navigation property for the search history records associated with this hub page.
    /// </summary>
    public virtual ICollection<SearchHistory> SearchHistory { get; set; } = new List<SearchHistory>();
}
