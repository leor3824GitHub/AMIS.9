using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;

public class GetIssuanceItemSpecs : Specification<IssuanceItem, IssuanceItemResponse>
{
    public GetIssuanceItemSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Product);
    }
}
