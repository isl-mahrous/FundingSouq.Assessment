namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the data transfer object (DTO) for an address.
/// </summary>
/// <remarks>
/// This DTO includes details of an address, such as street, city, country, and zip code,
/// as well as the associated client.
/// </remarks>
public class AddressDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the address.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the client associated with this address.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the country where this address is located.
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets the name of the country where this address is located.
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    /// Gets or sets the ID of the city where this address is located.
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the name of the city where this address is located.
    /// </summary>
    public string CityName { get; set; }

    /// <summary>
    /// Gets or sets the street address.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Gets or sets the zip code for this address.
    /// </summary>
    public string ZipCode { get; set; }
}
