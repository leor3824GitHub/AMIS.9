using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inspections.Specifications;

public sealed class InspectionItemExistsForPurchaseItemSpec : Specification<Inspection>
{
    public InspectionItemExistsForPurchaseItemSpec(Guid purchaseItemId)
    {
        Query.Where(i => i.Items.Any(item => item.PurchaseItemId == purchaseItemId))
             .Take(1); // Just need to know if any exist
    }
}