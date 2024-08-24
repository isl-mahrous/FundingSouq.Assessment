namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents the result of a JWT generation operation.
/// </summary>
/// <remarks>
/// This class contains the generated JWT token and its unique identifier (JTI).
/// </remarks>
public class JwtResultDto
{
    /// <summary>
    /// Gets or sets the generated JWT token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the JWT token (JTI).
    /// </summary>
    public string Jti { get; set; }
}