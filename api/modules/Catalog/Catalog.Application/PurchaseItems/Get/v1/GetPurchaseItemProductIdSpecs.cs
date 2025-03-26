using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;

public class GetPurchaseItemProductIdSpecs : Specification<PurchaseItem, PurchaseItemResponse>
{
    public GetPurchaseItemProductIdSpecs(Guid id)
    {
        Query
            .Where(p => p.ProductId == id);
    }
}
