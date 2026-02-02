using MediatR;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Cancel.v1;

/// <summary>
/// Command to cancel scheduled or in-progress maintenance
/// Records cancellation reason for audit trail
/// </summary>
public sealed record CancelMaintenanceCommand(
    Guid MaintenanceId,
    string? CancellationReason = null) : IRequest<CancelMaintenanceResponse>;

public sealed record CancelMaintenanceResponse(
    Guid MaintenanceId,
    string PropertyCode,
    string Status,
    DateTime CancelledOn);
