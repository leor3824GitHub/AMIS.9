using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Update.v1;

public sealed class UpdateAssetRequisitionHandler(
    [FromKeyedServices("inventories:assetrequisitions")] IRepository<AssetRequisition> repository)
    : IRequestHandler<UpdateAssetRequisitionCommand, UpdateAssetRequisitionResponse>
{
    public async Task<UpdateAssetRequisitionResponse> Handle(
        UpdateAssetRequisitionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assetRequisition = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Asset requisition {request.Id} not found");

        // Only update allowed fields
        if (request.ExpirationDate.HasValue)
        {
            assetRequisition.GetType().GetProperty(nameof(AssetRequisition.ExpirationDate))?
                .SetValue(assetRequisition, request.ExpirationDate);
        }

        await repository.UpdateAsync(assetRequisition, cancellationToken);

        return new UpdateAssetRequisitionResponse(
            assetRequisition.Id,
            assetRequisition.ExpirationDate,
            null);
    }
}

