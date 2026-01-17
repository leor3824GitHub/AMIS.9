using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.AcceptanceItems.Specifications;

public sealed class AcceptanceItemByPurchaseItemIdSpec : Specification<AcceptanceItem>
{
    public AcceptanceItemByPurchaseItemIdSpec(Guid purchaseItemId)
    {
        Query.Where(ai => ai.PurchaseItemId == purchaseItemId);
    }
}

