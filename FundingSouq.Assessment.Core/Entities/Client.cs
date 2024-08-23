using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

public class Client : User
{
    public string PersonalId { get; set; }
    public string ProfilePhoto { get; set; }
    public string MobileNumber { get; set; }
    public Gender Gender { get; set; }
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();

    public override List<Claim> GetClaims()
    {
        var claims = base.GetClaims();
        claims.Add(new Claim("PersonalId", PersonalId));
        claims.Add(new Claim("MobileNumber", MobileNumber));
        claims.Add(new Claim("Gender", Gender.ToString()));
        return claims;
    }
}