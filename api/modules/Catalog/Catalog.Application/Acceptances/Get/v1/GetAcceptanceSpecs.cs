using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;

public class GetAcceptanceSpecs : Specification<Acceptance, AcceptanceResponse>
{
    public GetAcceptanceSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.Purchase)
            .Include(i => i.Items)
            .Include(i => i.SupplyOfficer);
    }
}
