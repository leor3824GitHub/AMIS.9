using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Update.v1;

public sealed class UpdatePhysicalAssetHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<UpdatePhysicalAssetCommand, UpdatePhysicalAssetResponse>
{
    public async Task<UpdatePhysicalAssetResponse> Handle(
        UpdatePhysicalAssetCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var physicalAsset = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        // Update location if provided
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            physicalAsset.GetType().GetProperty(nameof(PhysicalAsset.Location))?
                .SetValue(physicalAsset, request.Location);
        }

        // Update condition if provided
        if (!string.IsNullOrWhiteSpace(request.Condition))
        {
            physicalAsset.GetType().GetProperty(nameof(PhysicalAsset.Condition))?
                .SetValue(physicalAsset, request.Condition);
        }

        // Update custodian if provided
        if (request.CurrentCustodianId.HasValue)
        {
            physicalAsset.GetType().GetProperty(nameof(PhysicalAsset.CurrentCustodianId))?
                .SetValue(physicalAsset, request.CurrentCustodianId);
        }

        await repository.UpdateAsync(physicalAsset, cancellationToken);

        return new UpdatePhysicalAssetResponse(
            physicalAsset.Id,
            physicalAsset.Location,
            physicalAsset.Condition,
            physicalAsset.CurrentCustodianId);
    }
}

