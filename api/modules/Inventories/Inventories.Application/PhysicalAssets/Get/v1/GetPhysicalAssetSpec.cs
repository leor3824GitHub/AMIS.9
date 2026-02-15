using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

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
            null, // ProductName - removed navigation
            p.AcquisitionCost,
            p.AcquisitionDate,
            p.CurrentClassification,
            p.Quantity,
            p.SerialNumber,
            p.ModelNumber,
            null, // Location - removed navigation
            p.AccumulatedDepreciation,
            p.BookValue,
            p.RCAAccountCode,
            null, // CurrentCustodianId - removed navigation
            null, // CurrentCustodianName - removed navigation
            p.ParentAssetId,
            null, // ParentAssetProductName - removed navigation
            null)); // ParentAssetPropertyCode - removed navigation
    }
}
