using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FundingSouq.Assessment.Api.Infrastructure;

public class FundingSouqControllerBase : ControllerBase
{
    /// <summary>
    /// Retrieves the user's Id from claims.
    /// </summary>
    /// <remarks>
    /// This method fetches the user's ID from claims associated with the authenticated user.
    /// It looks for a claim of type ClaimTypes.NameIdentifier
    /// and attempts to parse its value as an integer. If the parsing succeeds, the user's Id is returned.
    /// If the claim is missing or cannot be parsed, the method returns 0.
    /// </remarks>
    /// <returns>The user's ID retrieved from claims, or 0 if not found or parsing fails.</returns>
    [NonAction]
    protected int GetUserId()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? "0";
            return int.TryParse(userIdClaim, out var result) ? result : 0;
        }
        catch
        {
            return 0;
        }
    }
    
    /// <summary>
    /// Retrieves the user's type from claims.
    /// </summary>
    /// <remarks>
    /// This method fetches the user's type from claims associated with the authenticated user.
    /// It looks for a claim of type "user_type" and attempts to parse its value as an enum UserType.
    /// </remarks>
    /// <returns>The user's type retrieved from claims</returns>
    /// <exception cref="Exception">Throws exception if it failed to read the user_type claim from token</exception>
    [NonAction]
    protected UserType GetUserType()
    {
        var userTypeClaim = User.Claims.Where(claim => claim.Type == "user_type").FirstOrDefault();
        if(userTypeClaim is null)
            throw new Exception("User type claim not found");
        
        return Enum.Parse<UserType>(userTypeClaim.Value);
    }

    /// <summary>
    ///  Retrieves the user's role from claims.
    /// </summary>
    /// <returns>The user's role retrieved from claims</returns>
    /// <exception cref="Exception">Throws exception if it failed to read the role claim from token</exception>
    [NonAction]
    protected HubUserRole GetUserRole()
    {
        var userRoleClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        if(userRoleClaim is null)
            throw new Exception("User role claim not found");
        
        return Enum.Parse<HubUserRole>(userRoleClaim);
    }
}