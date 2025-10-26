using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Specifications;

public sealed class GetPurchaseItemWithAcceptancesSpec : Specification<PurchaseItem>
{
    public GetPurchaseItemWithAcceptancesSpec(Guid purchaseItemId)
    {
        Query.Where(pi => pi.Id == purchaseItemId)
             .Include(pi => pi.AcceptanceItems)
                .ThenInclude(ai => ai.Acceptance)
             .Include(pi => pi.InspectionItems);
    }
}
