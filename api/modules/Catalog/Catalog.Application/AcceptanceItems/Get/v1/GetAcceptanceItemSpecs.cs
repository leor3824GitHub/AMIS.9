using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;

public class GetAcceptanceItemSpecs : Specification<AcceptanceItem, AcceptanceItemResponse>
{
    public GetAcceptanceItemSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.PurchaseItem);
    }
}
