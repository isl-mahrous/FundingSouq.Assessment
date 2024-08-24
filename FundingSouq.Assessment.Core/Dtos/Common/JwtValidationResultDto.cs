using System.Security.Claims;

namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents the result of a JWT validation operation.
/// </summary>
/// <remarks>
/// This class contains the validation status of a JWT token, its unique identifier (JTI),
/// whether the token is expired, and the claims extracted from the token.
/// </remarks>
public class JwtValidationResultDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the JWT token (JTI).
    /// </summary>
    public string Jti { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the JWT token is valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the JWT token is expired.
    /// </summary>
    public bool IsExpired { get; set; }

    /// <summary>
    /// Gets or sets the list of claims extracted from the JWT token.
    /// </summary>
    public List<Claim> Claims { get; set; }
}
