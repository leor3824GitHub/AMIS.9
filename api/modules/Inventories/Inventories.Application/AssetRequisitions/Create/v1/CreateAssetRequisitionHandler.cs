using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Create.v1;

public sealed class CreateAssetRequisitionHandler(
    ILogger<CreateAssetRequisitionHandler> logger,
    [FromKeyedServices("inventories:assetrequisitions")] IRepository<AssetRequisition> repository)
    : IRequestHandler<CreateAssetRequisitionCommand, CreateAssetRequisitionResponse>
{
    public async Task<CreateAssetRequisitionResponse> Handle(CreateAssetRequisitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var assetRequisition = AssetRequisition.Create(
            request.EmployeeId,
            request.IssuanceId,
            DateTime.UtcNow,
            expirationDays: 30); // Default 30-day expiration
        await repository.AddAsync(assetRequisition, cancellationToken);
        
        logger.LogInformation("Asset requisition created {AssetRequisitionId}", assetRequisition.Id);
        return new CreateAssetRequisitionResponse(assetRequisition.Id);
    }
}

