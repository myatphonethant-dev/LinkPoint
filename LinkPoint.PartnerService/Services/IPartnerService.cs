using LinkPoint.PartnerService.Models;

namespace LinkPoint.PartnerService.Services;

public interface IPartnerService
{
    Task<IEnumerable<Tbl_Partner>> GetAll();

    Task<Tbl_Partner>? GetById(string id);

    Task Create(Tbl_Partner partner);

    Task Update(string id, Tbl_Partner partner);

    Task Delete(string id);
}