using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Inventories.Get.v1;

public class GetInventorySpecs : Specification<Inventory, InventoryResponse>
{
    public GetInventorySpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Product);
    }
}

