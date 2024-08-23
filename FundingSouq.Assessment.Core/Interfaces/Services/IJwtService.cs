using System.Security.Claims;
using FundingSouq.Assessment.Core.Dtos.Common;

namespace FundingSouq.Assessment.Core.Interfaces;

public interface IJwtService
{
    public JwtResultDto GenerateToken(List<Claim> claims);
    List<Claim> GetClaimsFromToken(string token);
    JwtValidationResultDto ValidateJwt(string jwtString);
}