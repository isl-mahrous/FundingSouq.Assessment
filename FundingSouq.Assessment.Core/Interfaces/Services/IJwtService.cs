using System.Security.Claims;
using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Interfaces;

/// <summary>
/// Provides methods for generating, validating, and retrieving claims from JWT tokens.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token based on the provided claims.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <returns>A <see cref="JwtResultDto"/> containing the generated token and its JTI.</returns>
    JwtResultDto GenerateToken(List<Claim> claims);

    /// <summary>
    /// Extracts claims from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>A list of claims contained in the token.</returns>
    List<Claim> GetClaimsFromToken(string token);

    /// <summary>
    /// Validates a JWT token.
    /// </summary>
    /// <param name="jwtString">The JWT token string to validate.</param>
    /// <returns>A <see cref="JwtValidationResultDto"/> indicating the validation result.</returns>
    JwtValidationResultDto ValidateJwt(string jwtString);
}
