using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Accept.v1;

public sealed class AcceptAssetRequisitionHandler(
    ILogger<AcceptAssetRequisitionHandler> logger,
    [FromKeyedServices("inventories:assetrequisitions")] IRepository<AssetRequisition> repository)
    : IRequestHandler<AcceptAssetRequisitionCommand, AcceptAssetRequisitionResponse>
{
    public async Task<AcceptAssetRequisitionResponse> Handle(AcceptAssetRequisitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assetRequisition = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Asset requisition {request.Id} not found");

        var signature = DigitalSignature.Create(
            request.SignatureData,
            Guid.Empty, // TODO: Get from current user context
            request.IpAddress,
            request.UserAgent,
            request.DeviceFingerprint);

        assetRequisition.Accept(signature);
        await repository.UpdateAsync(assetRequisition, cancellationToken);

        logger.LogInformation("Asset requisition {AssetRequisitionId} accepted", assetRequisition.Id);
        return new AcceptAssetRequisitionResponse(assetRequisition.Id);
    }
}

