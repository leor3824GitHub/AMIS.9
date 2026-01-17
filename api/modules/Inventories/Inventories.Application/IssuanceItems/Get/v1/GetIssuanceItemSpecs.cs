using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.IssuanceItems.Get.v1;

public class GetIssuanceItemSpecs : Specification<IssuanceItem, IssuanceItemResponse>
{
    public GetIssuanceItemSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Product);
    }
}

