using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Search.v1;

public sealed class SearchPhysicalAssetsSpecs : Specification<PhysicalAsset, PhysicalAssetResponse>
{
    public SearchPhysicalAssetsSpecs(SearchPhysicalAssetsCommand request)
    {
        Query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderByDescending(x => x.Created);

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

