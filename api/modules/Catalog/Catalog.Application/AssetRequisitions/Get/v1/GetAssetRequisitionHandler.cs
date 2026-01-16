using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AssetRequisitions.Get.v1;

public sealed class GetAssetRequisitionHandler(
    ILogger<GetAssetRequisitionHandler> logger,
    [FromKeyedServices("catalog:assetrequisitions")] IReadRepository<AssetRequisition> repository)
    : IRequestHandler<GetAssetRequisitionCommand, GetAssetRequisitionResponse>
{
    public async Task<GetAssetRequisitionResponse> Handle(GetAssetRequisitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requisition = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Asset requisition {request.Id} not found");

        logger.LogInformation("Retrieved asset requisition {AssetRequisitionId}", requisition.Id);
        return new GetAssetRequisitionResponse(
            requisition.Id,
            requisition.EmployeeId,
            requisition.IssuanceId,
            requisition.RequisitionDate,
            requisition.Status.ToString(),
            requisition.ExpirationDate,
            requisition.IsExpired);
    }
}
