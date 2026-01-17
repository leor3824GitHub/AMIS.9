using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Inventories.Get.v1;

public class GetInventoryProductIdSpecs : Specification<Inventory>, ISingleResultSpecification<Inventory>
{
    public GetInventoryProductIdSpecs(Guid? productId) => Query
            .Where(p => p.ProductId == productId);
}

