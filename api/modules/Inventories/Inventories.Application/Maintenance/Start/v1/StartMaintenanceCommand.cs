using MediatR;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Start.v1;

/// <summary>
/// Command to start maintenance work
/// Transitions from Scheduled to InProgress
/// Called when technician begins the work
/// </summary>
public sealed record StartMaintenanceCommand(
    Guid MaintenanceId) : IRequest<StartMaintenanceResponse>;

public sealed record StartMaintenanceResponse(
    Guid MaintenanceId,
    string PropertyCode,
    string Status,
    DateTime StartedOn);
