using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;

public sealed class GetInventoryRegistrySpecs : Specification<InventoryRegistry>
{
    public GetInventoryRegistrySpecs(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
