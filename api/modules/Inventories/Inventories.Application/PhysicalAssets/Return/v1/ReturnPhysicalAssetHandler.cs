using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Specifications;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Return.v1;

/// <summary>
/// Handler for returning a physical asset from an employee
/// </summary>
public sealed class ReturnPhysicalAssetHandler(
    ILogger<ReturnPhysicalAssetHandler> logger,
    IHttpContextAccessor httpContextAccessor,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<ReturnPhysicalAssetCommand, ReturnPhysicalAssetResponse>
{
    public async Task<ReturnPhysicalAssetResponse> Handle(ReturnPhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PhysicalAssetByIdSpec(request.Id);
        var asset = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        // Get current user ID from claims
        var currentUserId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Unable to determine current user.");

        if (!Guid.TryParse(currentUserId, out var userGuid))
            throw new UnauthorizedAccessException("Invalid user ID format.");

        // Assignment validation removed - CurrentCustodianId navigation no longer available
        // Assignment tracking moved to separate aggregate

        asset.Return(request.Reason, request.Condition, request.AcceptedBy, request.QuantityReturned);

        try
        {
            await repository.UpdateAsync(asset, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            logger.LogWarning(
                "Concurrency conflict while returning asset {AssetId}. Another process may be modifying this asset.",
                asset.Id);

            throw new InvalidOperationException(
                "Asset was updated by another process. Please refresh the page and try again.");
        }

        logger.LogInformation(
            "Physical asset {AssetId} returned. Reason: {Reason}, Condition: {Condition}",
            asset.Id,
            request.Reason,
            request.Condition);

        return new ReturnPhysicalAssetResponse
        {
            AssetId = asset.Id,
            AssignmentHistoryId = Guid.NewGuid(), // Placeholder - assignment tracking moved to separate service
            ReturnNotes = request.Reason,
            ReturnedDate = DateTime.UtcNow,
            Message = "Asset returned successfully"
        };
    }
}

