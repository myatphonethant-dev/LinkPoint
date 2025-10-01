namespace LinkPoint.IdentityService.Models;

public class LoginResponseModel
{
    public string Token { get; set; } = default!;

    public DateTime ExpiresAt { get; set; }
}