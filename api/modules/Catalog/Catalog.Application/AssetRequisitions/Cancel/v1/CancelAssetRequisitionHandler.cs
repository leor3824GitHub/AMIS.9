using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.AssetRequisitions.Cancel.v1;

public sealed class CancelAssetRequisitionHandler(
    [FromKeyedServices("catalog:assetrequisitions")] IRepository<AssetRequisition> repository)
    : IRequestHandler<CancelAssetRequisitionCommand, CancelAssetRequisitionResponse>
{
    public async Task<CancelAssetRequisitionResponse> Handle(
        CancelAssetRequisitionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assetRequisition = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Asset requisition {request.Id} not found");

        assetRequisition.Cancel();
        await repository.UpdateAsync(assetRequisition, cancellationToken);

        return new CancelAssetRequisitionResponse(
            assetRequisition.Id,
            assetRequisition.Status.ToString());
    }
}
