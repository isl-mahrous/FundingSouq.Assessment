using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents a hub user in the application.
/// </summary>
/// <remarks>
/// Inherits from <see cref="User"/> and adds additional properties specific to hub users, such as <see cref="Role"/> 
/// and <see cref="SearchHistory"/>.
/// </remarks>
public class HubUser : User
{
    /// <summary>
    /// Gets or sets the role of the hub user.
    /// </summary>
    public HubUserRole Role { get; set; }

    /// <summary>
    /// Gets or sets the collection of search history records associated with the hub user.
    /// </summary>
    public ICollection<SearchHistory> SearchHistory { get; set; }

    /// <summary>
    /// Retrieves a list of claims associated with the hub user.
    /// </summary>
    /// <remarks>
    /// This method overrides the base <see cref="User.GetClaims"/> method to include additional claims 
    /// specific to hub users, such as the role.
    /// </remarks>
    /// <returns>A list of <see cref="Claim"/> objects representing the hub user's claims.</returns>
    public override List<Claim> GetClaims()
    {
        var claims = base.GetClaims();
        claims.Add(new Claim(ClaimTypes.Role, Role.ToString()));
        return claims;
    }
}
