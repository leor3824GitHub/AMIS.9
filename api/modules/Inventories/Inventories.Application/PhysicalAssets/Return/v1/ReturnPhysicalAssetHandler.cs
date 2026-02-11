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

        // Validate that the asset is assigned to the current user
        if (asset.CurrentCustodianId != userGuid)
            throw new InvalidOperationException("You can only return assets that are assigned to you.");

        asset.Return(request.Reason, request.Condition, request.AcceptedBy, request.QuantityReturned);

        try
        {
            await repository.UpdateAsync(asset, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency conflict by reloading the entity
            logger.LogWarning(
                "Concurrency conflict while returning asset {AssetId}. Reloading and retrying.",
                asset.Id);

            // Reload the asset from the database
            var reloadedAsset = await repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

            // Validate again after reload
            if (reloadedAsset.CurrentCustodianId != userGuid)
                throw new InvalidOperationException("You can only return assets that are assigned to you.");

            // Re-apply the return operation
            reloadedAsset.Return(request.Reason, request.Condition, request.AcceptedBy, request.QuantityReturned);

            // Try update again
            await repository.UpdateAsync(reloadedAsset, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            asset = reloadedAsset;
        }

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

