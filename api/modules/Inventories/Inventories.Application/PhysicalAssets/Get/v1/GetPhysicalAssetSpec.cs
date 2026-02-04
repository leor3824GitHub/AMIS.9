using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;

public sealed class GetPhysicalAssetSpec : Specification<PhysicalAsset, PhysicalAssetResponse>
{
    public GetPhysicalAssetSpec(Guid id)
    {
        Query.Where(p => p.Id == id);

        Query.Select(p => new PhysicalAssetResponse(
            p.Id,
            p.PropertyCode,
            p.ProductId,
            p.Description,
            p.AcquisitionCost,
            p.AcquisitionDate,
            p.EstimatedUsefulLife,
            p.CurrentClassification,
            p.Quantity,
            p.UnitOfMeasure,
            p.SerialNumber,
            p.ModelNumber,
            p.AssignmentHistory
                .OrderByDescending(h => h.AssignmentDate)
                .Select(h => h.Location)
                .FirstOrDefault(),
            p.PPEType,
            p.AccumulatedDepreciation,
            p.BookValue,
            p.RCAAccountCode,
            p.CurrentCustodianId));
    }
}
