using LinkPoint.IdentityService.Models;
using LinkPoint.IdentityService.Services.Token;
using LinkPoint.Infrastructure.Data;
using MongoDB.Driver;
using static LinkPoint.SharedKernel.DevCode;

namespace LinkPoint.IdentityService.Services.User;

public class UserService : IUserRepository
{
    private readonly IMongoCollection<Tbl_User> _usersCollection;
    private readonly ITokenRepository _tokenRepository;

    public UserService(MongoDbContext dbContext, ITokenRepository tokenRepository)
    {
        _usersCollection = dbContext.GetCollection<Tbl_User>("Tbl_User");
        _tokenRepository = tokenRepository;
    }

    public async Task<Tbl_User?> GetByIdAsync(Guid userId)
        => await _usersCollection.Find(u => u.UserId == userId).FirstOrDefaultAsync();

    public async Task<Tbl_User?> GetByEmailAsync(string email)
        => await _usersCollection.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

    public async Task CreateAsync(Tbl_User user)
        => await _usersCollection.InsertOneAsync(user);

    public async Task UpdateAsync(Tbl_User user)
        => await _usersCollection.ReplaceOneAsync(u => u.UserId == user.UserId, user);

    public async Task<Tbl_User> RegisterAsync(RegisterRequestModel request)
    {
        var existing = await GetByEmailAsync(request.Email);
        if (existing is not null)
        {
            throw new Exception("Email already registered");
        }

        var user = new Tbl_User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };
        await CreateAsync(user);

        user.PasswordHash = null!;
        return user;
    }

    public async Task<LoginResponseModel> LoginAsync(LoginRequestModel request)
    {
        var user = await GetByEmailAsync(request.Email);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var ok = VerifyPassword(user.PasswordHash, request.Password);
        if (!ok) throw new UnauthorizedAccessException("Invalid credentials");

        var token = _tokenRepository.GenerateToken(user.UserId, user.Username);
        return new LoginResponseModel(token, DateTime.UtcNow.AddHours(1));
    }

    public async Task<Tbl_User?> UpdateProfileAsync(Guid userId, string? username, string? avatarUrl)
    {
        var user = await GetByIdAsync(userId);
        if (user is null)
        {
            throw new Exception("User not found");
        }

        user.Username = username ?? user.Username;
        user.AvatarUrl = avatarUrl ?? user.AvatarUrl;

        await UpdateAsync(user);
        user.PasswordHash = null!;
        return user;
    }
}