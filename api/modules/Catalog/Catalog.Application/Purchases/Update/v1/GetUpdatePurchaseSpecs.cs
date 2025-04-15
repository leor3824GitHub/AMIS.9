using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public class GetUpdatePurchaseSpecs : Specification<Purchase>
{
    public GetUpdatePurchaseSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Supplier)
            .Include(p => p.Items);
    }
}
