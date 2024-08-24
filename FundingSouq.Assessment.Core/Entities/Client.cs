using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a client user in the application.
/// </summary>
/// <remarks>
/// Inherits from <see cref="User"/> and adds additional properties specific to clients, such as <see cref="PersonalId"/>, 
/// <see cref="ProfilePhoto"/>, <see cref="MobileNumber"/>, <see cref="Gender"/>, <see cref="Addresses"/>, and <see cref="Accounts"/>.
/// </remarks>
public class Client : User
{
    /// <summary>
    /// Gets or sets the personal ID of the client.
    /// </summary>
    public string PersonalId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client's profile photo.
    /// </summary>
    public string ProfilePhoto { get; set; }

    /// <summary>
    /// Gets or sets the mobile number of the client.
    /// </summary>
    public string MobileNumber { get; set; }

    /// <summary>
    /// Gets or sets the gender of the client.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the collection of addresses associated with the client.
    /// </summary>
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    /// <summary>
    /// Gets or sets the collection of accounts associated with the client.
    /// </summary>
    public ICollection<Account> Accounts { get; set; } = new List<Account>();

    /// <summary>
    /// Retrieves a list of claims associated with the client.
    /// </summary>
    /// <remarks>
    /// This method overrides the base <see cref="User.GetClaims"/> method to include additional claims 
    /// specific to clients, such as personal ID, mobile number, and gender.
    /// </remarks>
    /// <returns>A list of <see cref="Claim"/> objects representing the client's claims.</returns>
    public override List<Claim> GetClaims()
    {
        var claims = base.GetClaims();
        claims.Add(new Claim("PersonalId", PersonalId));
        claims.Add(new Claim("MobileNumber", MobileNumber));
        claims.Add(new Claim("Gender", Gender.ToString()));
        return claims;
    }
}
