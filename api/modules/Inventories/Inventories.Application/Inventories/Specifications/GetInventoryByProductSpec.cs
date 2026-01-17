using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Inventories.Specifications;

public sealed class GetInventoryByProductSpec : Specification<Inventory>
{
    public GetInventoryByProductSpec(Guid productId)
    {
        Query.Where(i => i.ProductId == productId);
    }
}

