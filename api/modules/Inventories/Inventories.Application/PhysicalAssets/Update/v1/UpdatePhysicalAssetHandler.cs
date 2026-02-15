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

        // Update condition if provided
        if (!string.IsNullOrWhiteSpace(request.Condition))
        {
            physicalAsset.UpdateCondition(request.Condition);
        }

        // Update parent asset if provided
        if (request.ParentAssetId.HasValue)
        {
            physicalAsset.SetParentAsset(request.ParentAssetId.Value);
        }
        else if (request.ParentAssetId == null && physicalAsset.ParentAssetId.HasValue)
        {
            // Explicitly null ParentAssetId means clear it
            physicalAsset.ClearParentAsset();
        }

        await repository.UpdateAsync(physicalAsset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdatePhysicalAssetResponse(
            physicalAsset.Id,
            null, // Location - CurrentAssignment navigation removed
            physicalAsset.Condition);
    }
}

