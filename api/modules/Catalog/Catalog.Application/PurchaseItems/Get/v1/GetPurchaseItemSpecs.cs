using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;

public class GetPurchaseItemSpecs : Specification<PurchaseItem, PurchaseItemResponse>
{
    public GetPurchaseItemSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Purchase)
            .Include(p => p.Product);
    }
}
