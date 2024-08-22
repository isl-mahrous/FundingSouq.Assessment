using System.Security.Claims;
using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Entities;

public abstract class User : BaseEntity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public UserType UserType { get; set; }


    public virtual List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Name, $"{FirstName} {LastName}"),
            new Claim("UserType", UserType.ToString())
        };
    }
}