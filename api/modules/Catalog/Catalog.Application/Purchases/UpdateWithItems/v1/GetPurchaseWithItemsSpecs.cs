using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1;

internal sealed class GetPurchaseWithItemsSpecs : Specification<Purchase>, ISingleResultSpecification<Purchase>
{
    public GetPurchaseWithItemsSpecs(Guid id)
    {
        Query.AsTracking()
             .Where(x => x.Id == id)
             .Include(x => x.Items);
    }
}
