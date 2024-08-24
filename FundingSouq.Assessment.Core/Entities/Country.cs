namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a country entity within the application.
/// </summary>
/// <remarks>
/// This class contains details about a country, including its name, code, and phone prefix. 
/// It also tracks the cities associated with this country.
/// </remarks>
public class Country : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the country.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the code of the country, typically used as a country code in international contexts.
    /// <example>Saudi Arabia = SA</example>
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the phone prefix used for dialing numbers in this country.
    /// <example>Saudi Arabia = +966</example>
    /// </summary>
    public string PhonePrefix { get; set; }

    /// <summary>
    /// Navigation property for the cities associated with this country.
    /// </summary>
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}