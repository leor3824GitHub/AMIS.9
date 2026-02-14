using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Specifications;

/// <summary>
/// Specification for fetching a PhysicalAsset with all related history collections eager loaded.
/// This ensures that CurrentAssignment and LastReclassification computed properties work correctly.
/// </summary>
public sealed class GetPhysicalAssetWithHistorySpec : Specification<PhysicalAsset>
{
    public GetPhysicalAssetWithHistorySpec(Guid assetId)
    {
        Query
            .Where(a => a.Id == assetId)
            .Include(a => a.AssignmentHistory)
            .Include(a => a.ReclassificationHistory);
    }
}
