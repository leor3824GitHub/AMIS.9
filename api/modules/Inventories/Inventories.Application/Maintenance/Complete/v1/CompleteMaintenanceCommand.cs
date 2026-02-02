using MediatR;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Complete.v1;

/// <summary>
/// Command to complete maintenance work
/// Records completion notes, findings, and actual cost
/// </summary>
public sealed record CompleteMaintenanceCommand(
    Guid MaintenanceId,
    string? CompletionNotes = null,
    string? FindingsNotes = null,
    decimal? ActualCost = null) : IRequest<CompleteMaintenanceResponse>;

public sealed record CompleteMaintenanceResponse(
    Guid MaintenanceId,
    string PropertyCode,
    string Status,
    DateTime CompletedOn,
    decimal? ActualCost);
