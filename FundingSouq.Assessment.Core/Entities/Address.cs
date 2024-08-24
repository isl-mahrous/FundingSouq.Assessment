namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a physical address associated with a client in the application.
/// </summary>
/// <remarks>
/// This class contains details about an address, including the street, zip code, 
/// and the associated client, country, and city.
/// </remarks>
public class Address : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the client to whom this address belongs.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the country where this address is located.
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the city where this address is located.
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the street address.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Gets or sets the zip code for this address.
    /// </summary>
    public string ZipCode { get; set; }
    
    /// <summary>
    /// Navigation property for the client associated with this address.
    /// </summary>
    public virtual Client Client { get; set; }

    /// <summary>
    /// Navigation property for the country where this address is located.
    /// </summary>
    public virtual Country Country { get; set; }

    /// <summary>
    /// Navigation property for the city where this address is located.
    /// </summary>
    public virtual City City { get; set; }
}
