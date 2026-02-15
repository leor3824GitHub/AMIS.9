using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Specifications;

/// <summary>
/// Specification for fetching a PhysicalAsset by ID for write operations.
/// NOTE: Does NOT include AssignmentHistory or Product to avoid change tracking issues.
/// These are loaded separately only when needed for read operations.
/// </summary>
public sealed class PhysicalAssetByIdSpec : Specification<PhysicalAsset>
{
    public PhysicalAssetByIdSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id);
            // Intentionally NOT including AssignmentHistory or Product
            // to prevent change tracking conflicts during SaveChangesAsync
    }
}
