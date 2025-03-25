using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Issuances.Get.v1;

public class GetIssuanceSpecs : Specification<Issuance, IssuanceResponse>
{
    public GetIssuanceSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Product)
            .Include(p => p.Employee);
    }
}
