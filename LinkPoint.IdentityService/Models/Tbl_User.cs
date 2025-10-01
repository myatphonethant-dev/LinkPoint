using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinkPoint.IdentityService.Models;

public class Tbl_User
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public string? AvatarUrl { get; set; }

    public DateTime? BirthDate { get; set; }
}