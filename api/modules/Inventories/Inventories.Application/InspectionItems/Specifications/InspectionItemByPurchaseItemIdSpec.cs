using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InspectionItems.Specifications;

public sealed class InspectionItemByPurchaseItemIdSpec : Specification<InspectionItem>
{
    public InspectionItemByPurchaseItemIdSpec(Guid purchaseItemId)
    {
        Query.Where(ii => ii.PurchaseItemId == purchaseItemId);
    }
}

