using LinkPoint.PartnerService.Models;

namespace LinkPoint.PartnerService.Services;

public interface IPartnerService
{
    List<Tbl_Partner> GetAll();

    Tbl_Partner? GetById(string id);

    void Create(Tbl_Partner partner);

    void Update(string id, Tbl_Partner partner);

    void Delete(string id);
}