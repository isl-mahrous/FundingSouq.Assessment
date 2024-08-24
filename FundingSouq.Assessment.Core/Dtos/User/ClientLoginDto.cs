using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the data returned upon successful login of a client user.
/// </summary>
/// <remarks>
/// This DTO extends <see cref="UserLoginDto"/> and includes additional client-specific details 
/// such as personal ID, profile photo, mobile number, and gender.
/// </remarks>
public class ClientLoginDto : UserLoginDto
{
    /// <summary>
    /// Gets or sets the personal ID of the client.
    /// </summary>
    public string PersonalId { get; set; }

    /// <summary>
    /// Gets or sets the URL or path to the client's profile photo.
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
}