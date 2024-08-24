namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents global configuration settings for the application.
/// </summary>
/// <remarks>
/// This class is used to store global configuration values, such as the CDN URL, 
/// which are loaded from the application's configuration (e.g., appsettings.json) 
/// using the Options pattern at application startup.
/// </remarks>
public class Globals
{
    /// <summary>
    /// Gets or sets the base URL for the Content Delivery Network (CDN).
    /// </summary>
    public string CdnUrl { get; set; }
}