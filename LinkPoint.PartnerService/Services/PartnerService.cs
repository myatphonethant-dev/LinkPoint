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

    public async Task<IEnumerable<Tbl_Partner>> GetAll() => 
        await _partners.Find(_ => true).ToListAsync();

    public async Task<Tbl_Partner>? GetById(string id) =>
        await _partners.Find(p => p.PartnerId == id).FirstOrDefaultAsync();

    public async Task Create(Tbl_Partner partner) => await _partners.InsertOneAsync(partner);

    public async Task Update(string id, Tbl_Partner partner) =>
        await _partners.ReplaceOneAsync(p => p.PartnerId == id, partner);

    public async Task Delete(string id) =>
        await _partners.DeleteOneAsync(p => p.PartnerId == id);
}