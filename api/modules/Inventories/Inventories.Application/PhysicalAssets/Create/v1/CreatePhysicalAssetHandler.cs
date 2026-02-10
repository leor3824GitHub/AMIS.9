using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Create.v1;

public sealed class CreatePhysicalAssetHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<CreatePhysicalAssetCommand, CreatePhysicalAssetResponse>
{
    public async Task<CreatePhysicalAssetResponse> Handle(
        CreatePhysicalAssetCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var physicalAsset = PhysicalAsset.Create(
            request.PropertyCode,
            request.ProductId,
            request.AcquisitionCost,
            request.AcquisitionDate,
            request.Quantity,
            request.SerialNumber,
            request.ModelNumber);

        // Set parent asset if provided
        if (request.ParentAssetId.HasValue)
        {
            physicalAsset.SetParentAsset(request.ParentAssetId.Value);
        }

        await repository.AddAsync(physicalAsset, cancellationToken);

        return new CreatePhysicalAssetResponse(
            physicalAsset.Id,
            physicalAsset.PropertyCode,
            physicalAsset.AcquisitionCost,
            physicalAsset.GetCurrentClassification(),
            physicalAsset.ParentAssetId);
    }
}

