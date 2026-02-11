using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.UpdateCondition.v1;

public sealed class UpdateConditionHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<UpdateConditionCommand, UpdateConditionResponse>
{
    public async Task<UpdateConditionResponse> Handle(UpdateConditionCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.AssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset with ID {request.AssetId} not found.");

        asset.UpdateCondition(request.Condition, request.Remarks);

        await repository.UpdateAsync(asset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateConditionResponse(
            asset.Id,
            asset.Condition,
            DateTime.UtcNow);
    }
}

