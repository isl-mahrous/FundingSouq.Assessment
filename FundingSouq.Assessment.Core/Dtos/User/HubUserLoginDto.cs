using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the data returned upon successful login of a hub user.
/// </summary>
/// <remarks>
/// This DTO extends <see cref="UserLoginDto"/> and includes additional details specific to hub users,
/// such as their role within the hub.
/// </remarks>
public class HubUserLoginDto : UserLoginDto
{
    /// <summary>
    /// Gets or sets the role of the hub user (e.g., Admin, User).
    /// </summary>
    public HubUserRole Role { get; set; }
}