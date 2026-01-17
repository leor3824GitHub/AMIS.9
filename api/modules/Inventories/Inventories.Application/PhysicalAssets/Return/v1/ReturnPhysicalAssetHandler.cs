using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Return.v1;

/// <summary>
/// Handler for returning a physical asset from an employee
/// </summary>
public sealed class ReturnPhysicalAssetHandler(
    ILogger<ReturnPhysicalAssetHandler> logger,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<ReturnPhysicalAssetCommand, ReturnPhysicalAssetResponse>
{
    public async Task<ReturnPhysicalAssetResponse> Handle(ReturnPhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        asset.Return(request.Reason, request.Condition, request.AcceptedBy, request.QuantityReturned);
        await repository.UpdateAsync(asset, cancellationToken);

        var currentAssignment = asset.CurrentAssignment;

        logger.LogInformation(
            "Physical asset {AssetId} returned. Reason: {Reason}, Condition: {Condition}",
            asset.Id,
            request.Reason,
            request.Condition);

        return new ReturnPhysicalAssetResponse
        {
            AssetId = asset.Id,
            AssignmentHistoryId = currentAssignment?.Id ?? Guid.Empty,
            ReturnNotes = request.Reason,
            ReturnedDate = DateTime.UtcNow,
            Message = "Asset returned successfully"
        };
    }
}

