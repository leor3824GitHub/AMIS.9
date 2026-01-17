using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Specifications;

public sealed class AcceptanceWithItemsSpec : Specification<Acceptance>
{
    public AcceptanceWithItemsSpec(Guid acceptanceId)
    {
        Query
            .Where(a => a.Id == acceptanceId)
            .Include(a => a.Items)
                .ThenInclude(i => i.PurchaseItem)
                    .ThenInclude(pi => pi.Product)
            .Include(a => a.Purchase);
    }
}

