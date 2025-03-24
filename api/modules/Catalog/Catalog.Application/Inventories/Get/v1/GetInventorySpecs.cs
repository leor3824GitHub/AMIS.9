using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;

public class GetInventorySpecs : Specification<Inventory, InventoryResponse>
{
    public GetInventorySpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Product);
    }
}
