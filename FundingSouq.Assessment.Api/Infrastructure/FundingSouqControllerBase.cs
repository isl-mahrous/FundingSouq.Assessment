using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FundingSouq.Assessment.Api.Infrastructure;

public class FundingSouqControllerBase : ControllerBase
{
    /// <summary>
    /// Retrieves the user's ID from claims.
    /// </summary>
    /// <returns>The user's ID, or 0 if not found or parsing fails.</returns>
    [NonAction]
    protected int GetUserId()
    {
        // Attempt to parse the user ID from claims; return 0 if unsuccessful
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "0";
        return int.TryParse(userIdClaim, out var result) ? result : 0;
    }
    
    /// <summary>
    /// Retrieves the user's type from claims.
    /// </summary>
    /// <returns>The user's type.</returns>
    /// <exception cref="Exception">Thrown if the user type claim is missing.</exception>
    [NonAction]
    protected UserType GetUserType()
    {
        // Get the user type from claims; throw an exception if not found
        var userTypeClaim = User.Claims.FirstOrDefault(claim => claim.Type == "user_type");
        if (userTypeClaim is null)
            throw new Exception("User type claim not found");
        
        return Enum.Parse<UserType>(userTypeClaim.Value);
    }

    /// <summary>
    /// Retrieves the user's role from claims.
    /// </summary>
    /// <returns>The user's role.</returns>
    /// <exception cref="Exception">Thrown if the role claim is missing.</exception>
    [NonAction]
    protected HubUserRole GetUserRole()
    {
        // Get the user role from claims; throw an exception if not found
        var userRoleClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        if (userRoleClaim is null)
            throw new Exception("User role claim not found");
        
        return Enum.Parse<HubUserRole>(userRoleClaim);
    }
}
