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
            request.Classification,
            request.PropertyCode,
            request.ProductId,
            request.Description,
            request.AcquisitionCost,
            request.AcquisitionDate,
            request.EstimatedUsefulLife,
            request.Quantity,
            request.UnitOfMeasure,
            request.SerialNumber,
            request.ModelNumber,
            request.PPEType);

        await repository.AddAsync(physicalAsset, cancellationToken);

        return new CreatePhysicalAssetResponse(
            physicalAsset.Id,
            physicalAsset.PropertyCode,
            physicalAsset.Description,
            physicalAsset.AcquisitionCost,
            physicalAsset.CurrentClassification);
    }
}

