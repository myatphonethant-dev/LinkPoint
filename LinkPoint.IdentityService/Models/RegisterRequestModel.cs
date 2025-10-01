namespace LinkPoint.IdentityService.Models;

public class RegisterRequestModel
{
    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}