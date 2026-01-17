using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Delete.v1;

public sealed record DeleteAssetRequisitionCommand(Guid Id) : IRequest<DeleteAssetRequisitionResponse>;

public sealed record DeleteAssetRequisitionResponse(Guid Id);

public sealed class DeleteAssetRequisitionHandler(
    ILogger<DeleteAssetRequisitionHandler> logger,
    [FromKeyedServices("inventories:assetrequisitions")] IRepository<AssetRequisition> repository)
    : IRequestHandler<DeleteAssetRequisitionCommand, DeleteAssetRequisitionResponse>
{
    public async Task<DeleteAssetRequisitionResponse> Handle(DeleteAssetRequisitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requisition = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Asset requisition {request.Id} not found");

        await repository.DeleteAsync(requisition, cancellationToken);

        logger.LogInformation("Asset requisition {AssetRequisitionId} deleted", requisition.Id);
        return new DeleteAssetRequisitionResponse(requisition.Id);
    }
}

