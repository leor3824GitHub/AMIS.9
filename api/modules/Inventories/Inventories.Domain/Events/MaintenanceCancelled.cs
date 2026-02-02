using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when scheduled maintenance is cancelled
/// Maintains audit trail of maintenance decisions
/// </summary>
public sealed record MaintenanceCancelled : DomainEvent
{
    public AssetMaintenance? Maintenance { get; set; }
    public string? CancellationReason { get; set; }
}
