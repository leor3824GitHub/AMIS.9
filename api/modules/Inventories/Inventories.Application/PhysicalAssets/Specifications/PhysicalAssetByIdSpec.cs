using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Specifications;

/// <summary>
/// Specification for fetching a PhysicalAsset by ID with all necessary navigation properties
/// for write operations (Issue, Return, Transfer, etc.)
/// </summary>
public sealed class PhysicalAssetByIdSpec : Specification<PhysicalAsset>
{
    public PhysicalAssetByIdSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.AssignmentHistory)
            .Include(p => p.Product);
    }
}
