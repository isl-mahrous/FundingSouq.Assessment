using System.Security.Claims;

namespace FundingSouq.Assessment.Core.Dtos.Common;

public class JwtResultDto
{
    public string Token { get; set; }
    public string Jti { get; set; }
}

public class JwtValidationResultDto
{
    public string Jti { get; set; }
    public bool IsValid { get; set; }
    public bool IsExpired { get; set; }
    public List<Claim> Claims { get; set; }
}