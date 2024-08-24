using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the data returned upon successful user login.
/// </summary>
/// <remarks>
/// This DTO includes basic user information such as ID, email, first and last name, user type, 
/// authentication token, and timestamps for creation and last modification.
/// </remarks>
public class UserLoginDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

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
    /// Gets or sets the type of the user (e.g., HubUser, Client).
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// Gets or sets the JWT token issued for the user after a successful login.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the timestamp for when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp for when the user was last modified.
    /// </summary>
    public DateTime LastModifiedAt { get; set; }
}