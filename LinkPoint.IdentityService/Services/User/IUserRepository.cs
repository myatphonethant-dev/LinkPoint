using LinkPoint.IdentityService.Models;

namespace LinkPoint.IdentityService.Services.User;

public interface IUserRepository
{
    Task<Tbl_User?> GetByIdAsync(Guid userId);
    Task<Tbl_User?> GetByEmailAsync(string email);
    Task CreateAsync(Tbl_User user);
    Task UpdateAsync(Tbl_User user);

    Task<Tbl_User> RegisterAsync(RegisterRequestModel request);
    Task<LoginResponseModel> LoginAsync(LoginRequestModel request);
    Task<Tbl_User?> UpdateProfileAsync(Guid userId, string? username, string? avatarUrl);
}