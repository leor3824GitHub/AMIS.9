using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Specifications;
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

        // Use specification to eagerly load assignment history
        var spec = new GetPhysicalAssetWithHistorySpec(request.Id);
        var asset = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        // Capture current assignment BEFORE calling Return() since Return() changes the status to "Returned"
        var currentAssignment = asset.CurrentAssignment;
        if (currentAssignment is null)
            throw new InvalidOperationException($"Physical asset {request.Id} is not currently assigned to an employee");

        asset.Return(request.Reason, request.Condition, request.AcceptedBy, request.QuantityReturned);
        await repository.UpdateAsync(asset, cancellationToken);

        logger.LogInformation(
            "Physical asset {AssetId} returned. Reason: {Reason}, Condition: {Condition}",
            asset.Id,
            request.Reason,
            request.Condition);

        return new ReturnPhysicalAssetResponse
        {
            AssetId = asset.Id,
            AssignmentHistoryId = currentAssignment.Id,
            ReturnNotes = request.Reason,
            ReturnedDate = DateTime.UtcNow,
            Message = "Asset returned successfully"
        };
    }
}

