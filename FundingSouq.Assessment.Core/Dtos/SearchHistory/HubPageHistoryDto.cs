namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents a single search entry in the history of a hub page.
/// </summary>
/// <remarks>
/// This DTO contains the ID of the search entry, the search query string, and the date when the search was performed.
/// </remarks>
public class HubPageHistoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the search entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the search query string used in the search entry.
    /// </summary>
    public string SearchQuery { get; set; }

    /// <summary>
    /// Gets or sets the date when the search was performed.
    /// </summary>
    public DateTime SearchDate { get; set; }
}