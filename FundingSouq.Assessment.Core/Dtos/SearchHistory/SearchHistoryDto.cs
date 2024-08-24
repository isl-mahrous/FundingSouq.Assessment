namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the search history for a specific hub page.
/// </summary>
/// <remarks>
/// This DTO contains the ID and key of the hub page, along with a list of search history entries 
/// associated with that page.
/// </remarks>
public class SearchHistoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the hub page associated with this search history.
    /// </summary>
    public int HubPageId { get; set; }

    /// <summary>
    /// Gets or sets the key of the hub page associated with this search history.
    /// </summary>
    public string HubPageKey { get; set; }

    /// <summary>
    /// Gets or sets the list of search history entries for the hub page.
    /// </summary>
    public List<HubPageHistoryDto> History { get; set; }
}