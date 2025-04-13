using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;

public class GetInventoryProductIdSpecs : Specification<Inventory>, ISingleResultSpecification<Inventory>
{
    public GetInventoryProductIdSpecs(Guid? productId) => Query
            .Where(p => p.ProductId == productId);
}
