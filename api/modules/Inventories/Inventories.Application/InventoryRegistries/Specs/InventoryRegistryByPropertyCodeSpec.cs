using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;

public sealed class InventoryRegistryByPropertyCodeSpec : Specification<InventoryRegistry>
{
    public InventoryRegistryByPropertyCodeSpec(string propertyCode)
    {
        // Normalize comparison to avoid case/whitespace mismatches
        var normalized = propertyCode.Trim().ToUpper();
        Query.Where(x => x.PropertyCode.Trim().ToUpper() == normalized);
    }
}
