using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

public class GetPurchaseSpecs : Specification<Purchase, PurchaseResponse>
{
    public GetPurchaseSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Supplier);
    }
}
