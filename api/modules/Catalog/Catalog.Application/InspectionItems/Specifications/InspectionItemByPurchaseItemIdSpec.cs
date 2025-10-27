using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Specifications;

public sealed class InspectionItemByPurchaseItemIdSpec : Specification<InspectionItem>
{
    public InspectionItemByPurchaseItemIdSpec(Guid purchaseItemId)
    {
        Query.Where(ii => ii.PurchaseItemId == purchaseItemId);
    }
}
