using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;

public sealed class GetPhysicalAssetHandler(
    [FromKeyedServices("inventories:physicalassets")] IReadRepository<PhysicalAsset> repository)
    : IRequestHandler<GetPhysicalAssetCommand, PhysicalAssetResponse>
{
    public async Task<PhysicalAssetResponse> Handle(
        GetPhysicalAssetCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var physicalAsset = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        return new PhysicalAssetResponse(
            physicalAsset.Id,
            physicalAsset.PropertyCode,
            physicalAsset.ProductId,
            physicalAsset.Description,
            physicalAsset.AcquisitionCost,
            physicalAsset.AcquisitionDate,
            physicalAsset.EstimatedUsefulLife,
            physicalAsset.CurrentClassification,
            physicalAsset.Quantity,
            physicalAsset.UnitOfMeasure,
            physicalAsset.SerialNumber,
            physicalAsset.ModelNumber,
            physicalAsset.CurrentAssignment?.Location,
            physicalAsset.PPEType,
            physicalAsset.AccumulatedDepreciation,
            physicalAsset.BookValue,
            physicalAsset.RCAAccountCode,
            physicalAsset.CurrentCustodianId);
    }
}

