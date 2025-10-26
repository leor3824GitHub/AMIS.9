using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Specifications;

public sealed class AcceptanceItemByPurchaseItemIdSpec : Specification<AcceptanceItem>
{
    public AcceptanceItemByPurchaseItemIdSpec(Guid purchaseItemId)
    {
        Query.Where(ai => ai.PurchaseItemId == purchaseItemId);
    }
}
