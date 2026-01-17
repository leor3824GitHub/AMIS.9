using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Issuances.Get.v1;

public class GetIssuanceSpecs : Specification<Issuance, IssuanceResponse>
{
    public GetIssuanceSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Employee);
    }
}

