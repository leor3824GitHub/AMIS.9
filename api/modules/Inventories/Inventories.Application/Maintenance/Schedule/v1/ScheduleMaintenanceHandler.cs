using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Schedule.v1;

/// <summary>
/// Handler for scheduling maintenance
/// Validates asset, maintenance type, and scheduled date
/// Creates AssetMaintenance entity in Scheduled state
/// Note: AssetMaintenance is saved as part of PhysicalAsset aggregate
/// </summary>
public sealed class ScheduleMaintenanceHandler(
    ILogger<ScheduleMaintenanceHandler> logger,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
    : IRequestHandler<ScheduleMaintenanceCommand, ScheduleMaintenanceResponse>
{
    public async Task<ScheduleMaintenanceResponse> Handle(
        ScheduleMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // TODO: Get from current user context
        var currentUserId = Guid.Empty;
        if (currentUserId == Guid.Empty)
            throw new UnauthorizedAccessException("User context is required.");

        // Validate asset exists
        var asset = await assetRepository.GetByIdAsync(request.PhysicalAssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.PhysicalAssetId} not found.");

        // Parse and validate maintenance type
        if (!Enum.TryParse<MaintenanceType>(request.MaintenanceType, ignoreCase: true, out var maintenanceType))
            throw new ArgumentException($"Invalid maintenance type: {request.MaintenanceType}");

        // Validate scheduled date
        if (request.ScheduledDate == default)
            throw new ArgumentException("Scheduled date must be provided.");

        // Validate scheduler exists
        var scheduler = await employeeRepository.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw new InvalidOperationException($"Employee {currentUserId} not found.");

        // Schedule maintenance
        var maintenance = AssetMaintenance.Schedule(
            asset.Id,
            asset.PropertyCode,
            asset.Description,
            maintenanceType,
            request.Description,
            request.ScheduledDate,
            scheduler.Id,
            request.EstimatedCost,
            request.CostReference);

        // Add maintenance to asset and save through asset repository
        asset.MaintenanceHistory.Add(maintenance);
        await assetRepository.UpdateAsync(asset, cancellationToken);
        await assetRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Maintenance scheduled for asset {AssetId} ({PropertyCode}). Type: {MaintenanceType}, Scheduled: {ScheduledDate}",
            asset.Id,
            asset.PropertyCode,
            maintenanceType,
            request.ScheduledDate);

        return new ScheduleMaintenanceResponse(
            maintenance.Id,
            maintenance.AssetPropertyCode,
            maintenance.Status.ToString(),
            maintenance.ScheduledDate);
    }
}
