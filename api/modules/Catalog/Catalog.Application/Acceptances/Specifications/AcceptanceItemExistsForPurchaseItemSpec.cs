using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Specifications;

public sealed class AcceptanceItemExistsForPurchaseItemSpec : Specification<Acceptance>
{
    public AcceptanceItemExistsForPurchaseItemSpec(Guid purchaseItemId)
    {
        Query.Where(a => a.Items.Any(item => item.PurchaseItemId == purchaseItemId))
             .Take(1); // Just need to know if any exist
    }
}