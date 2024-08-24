namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a city entity within the application.
/// </summary>
/// <remarks>
/// This class contains details about a city, including its name and the ID of the country it belongs to.
/// It also includes a navigation property to the associated country.
/// </remarks>
public class City : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the ID of the country to which this city belongs.
    /// </summary>
    public int CountryId { get; set; }
    
    /// <summary>
    /// Navigation property for the country to which this city belongs.
    /// </summary>
    public virtual Country Country { get; set; }
}
