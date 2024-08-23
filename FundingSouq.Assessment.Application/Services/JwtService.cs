using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FundingSouq.Assessment.Application.Services;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly TimeSpan _tokenLifeTime;
    private readonly string _audience;

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["JwtOptions:Key"];
        _issuer = configuration["JwtOptions:Issuer"];
        _audience = configuration["JwtOptions:Audience"];
        _tokenLifeTime = TimeSpan.FromHours(int.Parse(configuration["JwtOptions:LifeTimeInHours"]!));
    }
    public JwtResultDto GenerateToken(List<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var claimSubject = new ClaimsIdentity();

        // Basic Claims
        claimSubject.AddClaims(new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(CultureInfo.CurrentCulture))
        });

        // Custom Claims like roles and permissions
        if (claims is not null && claims.Count != 0)
        {
            claimSubject.AddClaims(claims);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimSubject,
            Expires = DateTime.UtcNow.Add(_tokenLifeTime),
            Issuer = _issuer,
            Audience = _audience,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512),

        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        return new JwtResultDto
        {
            Token = accessToken,
            Jti = token.Id
        };
    }

    public List<Claim> GetClaimsFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ReadJwtToken(token).Claims.ToList();
    }

    public JwtValidationResultDto ValidateJwt(string jwtString)
    {
        var validationResult = new JwtValidationResultDto();
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                
            };

            var claimsPrincipal = tokenHandler.ValidateToken(jwtString, validationParameters, out var validatedToken);
            validationResult.IsValid = true;
            validationResult.IsExpired = validatedToken.ValidTo < DateTime.UtcNow;
            validationResult.Claims = claimsPrincipal.Claims.ToList();
            validationResult.Jti = validatedToken.Id;
        }
        catch (SecurityTokenException)
        {
            // Token validation failed
            validationResult.IsValid = false;
            validationResult.IsExpired = false; // Not applicable since the token is not valid
            validationResult.Claims = null; // No claims to retrieve
        }

        return validationResult;
    }
}