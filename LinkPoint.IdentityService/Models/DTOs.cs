namespace LinkPoint.IdentityService.Models;

public record RegisterRequestModel(string Username, string Email, string Password);

public record LoginRequestModel(string Email, string Password);

public record LoginResponseModel(string Token, DateTime ExpiresAt);

public record UpdateProfileRequestModel(string? Username, string? AvatarUrl);