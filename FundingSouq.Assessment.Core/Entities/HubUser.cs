using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

public class HubUser : User
{
    public HubUserRole Role { get; set; }
    
    public ICollection<SearchHistory> SearchHistory { get; set; }

    public override List<Claim> GetClaims()
    {
        var claims = base.GetClaims();
        claims.Add(new Claim(ClaimTypes.Role, Role.ToString()));
        return claims;
    }
}