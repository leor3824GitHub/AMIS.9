using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Specifications;

public sealed class GetAcceptanceWithItemsSpec : Specification<Acceptance>
{
    public GetAcceptanceWithItemsSpec(Guid acceptanceId)
    {
        Query.Where(a => a.Id == acceptanceId)
             .Include(a => a.Items)
                .ThenInclude(ai => ai.PurchaseItem)
                    .ThenInclude(pi => pi.Product)
             .Include(a => a.Items)
                .ThenInclude(ai => ai.PurchaseItem)
                    .ThenInclude(pi => pi.AcceptanceItems);
    }
}
