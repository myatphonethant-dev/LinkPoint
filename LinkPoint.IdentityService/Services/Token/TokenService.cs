using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkPoint.IdentityService.Services.Token;

public class TokenService : ITokenRepository
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(Guid userId, string username)
    {
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");
        var issuer = _config["Jwt:Issuer"] ?? "LinkPoint.IdentityService";
        var audience = _config["Jwt:Audience"] ?? "LinkPoint.Services";
        var expiresHours = int.TryParse(_config["Jwt:ExpiresHours"], out var h) ? h : 1;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim("uid", userId.ToString())
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(expiresHours), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}