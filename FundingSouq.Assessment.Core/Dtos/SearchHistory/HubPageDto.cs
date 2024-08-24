namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents a hub page in the system.
/// </summary>
/// <remarks>
/// This DTO contains the ID and key of the hub page, which are used to uniquely identify and reference the page.
/// </remarks>
public class HubPageDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the hub page.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the key used to identify the hub page.
    /// </summary>
    public string Key { get; set; }
}