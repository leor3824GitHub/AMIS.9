using MediatR;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Schedule.v1;

/// <summary>
/// Command to schedule maintenance for an asset
/// Called by maintenance planner or supply officer
/// </summary>
public sealed record ScheduleMaintenanceCommand(
    Guid PhysicalAssetId,
    string MaintenanceType,
    string Description,
    DateTime ScheduledDate,
    decimal? EstimatedCost = null,
    string? CostReference = null) : IRequest<ScheduleMaintenanceResponse>;

public sealed record ScheduleMaintenanceResponse(
    Guid MaintenanceId,
    string PropertyCode,
    string Status,
    DateTime ScheduledDate);
