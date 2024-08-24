namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents a country with its associated details and cities.
/// </summary>
/// <remarks>
/// This DTO is used to transfer country data, including its ID, name, code, phone prefix, 
/// and a list of associated cities.
/// </remarks>
public class CountryDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the country.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the country.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the code of the country.
    /// </summary>
    /// <remarks>
    /// This code usually represents the ISO code for the country.
    /// </remarks>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the phone prefix for the country.
    /// </summary>
    /// <remarks>
    /// The phone prefix is used in international dialing to identify the country.
    /// </remarks>
    public string PhonePrefix { get; set; }

    /// <summary>
    /// Gets or sets the list of cities associated with the country.
    /// </summary>
    /// <remarks>
    /// This list contains the cities that belong to the country, providing a relationship between 
    /// the country and its cities.
    /// </remarks>
    public List<CityDto> Cities { get; set; } = new();
}