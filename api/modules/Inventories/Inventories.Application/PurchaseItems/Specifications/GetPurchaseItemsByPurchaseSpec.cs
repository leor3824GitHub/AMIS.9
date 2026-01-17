using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.PurchaseItems.Specifications;

public sealed class GetPurchaseItemsByPurchaseSpec : Specification<PurchaseItem>
{
    public GetPurchaseItemsByPurchaseSpec(Guid purchaseId)
    {
        Query.Where(pi => pi.PurchaseId == purchaseId)
             .Include(pi => pi.AcceptanceItems)
                .ThenInclude(ai => ai.Acceptance)
             .Include(pi => pi.InspectionItems);
    }
}

