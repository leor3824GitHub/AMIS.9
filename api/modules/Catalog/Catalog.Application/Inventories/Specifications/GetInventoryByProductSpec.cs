using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inventories.Specifications;

public sealed class GetInventoryByProductSpec : Specification<Inventory>
{
    public GetInventoryByProductSpec(Guid productId)
    {
        Query.Where(i => i.ProductId == productId);
    }
}
