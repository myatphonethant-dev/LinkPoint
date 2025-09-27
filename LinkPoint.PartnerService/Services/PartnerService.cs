using LinkPoint.Infrastructure.Data;
using LinkPoint.PartnerService.Models;
using LinkPoint.PartnerService.Services;
using MongoDB.Driver;

namespace LinkPoint.PartnerService.Data;

public class PartnerService : IPartnerService
{
    private readonly IMongoCollection<Tbl_Partner> _partners;

    public PartnerService(MongoDbContext dbContext)
    {
        _partners = dbContext.GetCollection<Tbl_Partner>("Tbl_Partner");
    }

    public List<Tbl_Partner> GetAll() => _partners.Find(_ => true).ToList();

    public Tbl_Partner? GetById(string id) =>
        _partners.Find(p => p.PartnerId == id).FirstOrDefault();

    public void Create(Tbl_Partner partner) => _partners.InsertOne(partner);

    public void Update(string id, Tbl_Partner partner) =>
        _partners.ReplaceOne(p => p.PartnerId == id, partner);

    public void Delete(string id) =>
        _partners.DeleteOne(p => p.PartnerId == id);
}