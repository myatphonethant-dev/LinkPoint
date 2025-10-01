namespace LinkPoint.IdentityService.Services.Token;

public interface ITokenRepository
{
    string GenerateToken(Guid userId, string username);
}