namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents a city with its associated details and the country it belongs to.
/// </summary>
/// <remarks>
/// This DTO is used to transfer city data, including its ID, name, and the ID of the country it belongs to.
/// </remarks>
public class CityDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the city.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the ID of the country the city belongs to.
    /// </summary>
    /// <remarks>
    /// This property represents the foreign key relationship between the city and its country.
    /// </remarks>
    public int CountryId { get; set; }
}