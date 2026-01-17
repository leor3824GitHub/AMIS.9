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
            p.Description,
            p.AcquisitionCost,
            p.AcquisitionDate,
            p.EstimatedUsefulLife,
            p.CurrentClassification,
            p.Quantity,
            p.UnitOfMeasure,
            p.SerialNumber,
            p.ModelNumber,
            p.Location,
            p.PPEType,
            p.AccumulatedDepreciation,
            p.BookValue,
            p.RCAAccountCode,
            p.CurrentCustodianId));
    }
}

