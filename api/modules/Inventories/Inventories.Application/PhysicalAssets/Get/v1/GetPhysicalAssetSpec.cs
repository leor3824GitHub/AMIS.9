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
            p.Product.Name,
            p.AcquisitionCost,
            p.AcquisitionDate,
            p.CurrentClassification,
            p.Quantity,
            p.SerialNumber,
            p.ModelNumber,
            p.AssignmentHistory
                .OrderByDescending(h => h.AssignmentDate)
                .Select(h => h.Location)
                .FirstOrDefault(),
            p.AccumulatedDepreciation,
            p.BookValue,
            p.RCAAccountCode,
            p.AssignmentHistory
                .FirstOrDefault(h => h.Status == "Active") != null
                ? p.AssignmentHistory.FirstOrDefault(h => h.Status == "Active")!.EmployeeId
                : null,
            p.ParentAssetId,
            p.ParentAsset != null ? p.ParentAsset.Product.Name : null,
            p.ParentAsset != null ? p.ParentAsset.PropertyCode : null));
    }
}
