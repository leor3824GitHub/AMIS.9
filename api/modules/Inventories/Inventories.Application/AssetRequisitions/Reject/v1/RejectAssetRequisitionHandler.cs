using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Reject.v1;

public sealed class RejectAssetRequisitionHandler(
    [FromKeyedServices("inventories:assetrequisitions")] IRepository<AssetRequisition> repository)
    : IRequestHandler<RejectAssetRequisitionCommand, RejectAssetRequisitionResponse>
{
    public async Task<RejectAssetRequisitionResponse> Handle(
        RejectAssetRequisitionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assetRequisition = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Asset requisition {request.Id} not found");

        assetRequisition.Reject(request.Reason);
        await repository.UpdateAsync(assetRequisition, cancellationToken);

        return new RejectAssetRequisitionResponse(
            assetRequisition.Id,
            assetRequisition.Status.ToString(),
            assetRequisition.RejectionReason ?? string.Empty);
    }
}

