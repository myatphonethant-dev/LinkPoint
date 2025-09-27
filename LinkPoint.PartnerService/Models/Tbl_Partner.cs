using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinkPoint.PartnerService.Models;

public class Tbl_Partner
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string PartnerId { get; set; }

    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? PairCode { get; set; }

    public string? PairedWithId { get; set; }
}