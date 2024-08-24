namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a search history record associated with a hub user.
/// </summary>
/// <remarks>
/// This class contains details about a specific search performed by a hub user, 
/// including the search query, the date of the search, and the page on which the search was conducted.
/// </remarks>
public class SearchHistory : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the hub user who performed the search.
    /// </summary>
    public int HubUserId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the hub page on which the search was conducted.
    /// </summary>
    public int HubPageId { get; set; }

    /// <summary>
    /// Gets or sets the search query entered by the hub user. The query should be in the format of a query string
    /// <example>page=1&amp;pageSize=10&amp;sortKey=id&amp;sortDirection=1&amp;searchKey=abc</example>
    /// </summary>
    public string SearchQuery { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the search was performed.
    /// </summary>
    public DateTime SearchDate { get; set; }

    /// <summary>
    /// Navigation property for the hub user who performed the search.
    /// </summary>
    public virtual HubUser HubUser { get; set; }

    /// <summary>
    /// Navigation property for the hub page on which the search was conducted.
    /// </summary>
    public virtual HubPage HubPage { get; set; }
}