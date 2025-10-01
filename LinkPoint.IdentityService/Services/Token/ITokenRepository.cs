using System.Security.Claims;

namespace LinkPoint.IdentityService.Services.Token;

public interface ITokenRepository
{
    string GenerateToken(Guid userId, string username);
    ClaimsPrincipal ValidateToken(string token);
}