using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;

public sealed class InventoryRegistryByPropertyCodeSpec : Specification<InventoryRegistry>
{
    public InventoryRegistryByPropertyCodeSpec(string propertyCode)
    {
        Query.Where(x => x.PropertyCode == propertyCode);
    }
}
