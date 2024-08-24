using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

/// <summary>
/// Represents the base class for all user entities in the application.
/// </summary>
/// <remarks>
/// This class provides common properties and methods that are inherited by all user types in the system, such as <see cref="Email"/>, <see cref="FirstName"/>, 
/// <see cref="LastName"/>, <see cref="PasswordHash"/>, and <see cref="UserType"/>.
/// </remarks>
public abstract class User : BaseEntity
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the hashed password of the user.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the type of user.
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// Retrieves a list of claims associated with the user.
    /// </summary>
    /// <remarks>
    /// This method returns a list of standard claims, including the user's ID, email, full name, and user type.
    /// It can be overridden by derived classes to include additional claims.
    /// </remarks>
    /// <returns>A list of <see cref="Claim"/> objects representing the user's claims.</returns>
    public virtual List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Name, $"{FirstName} {LastName}"),
            new Claim("user_type", UserType.ToString())
        };
    }
}
