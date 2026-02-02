using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when scheduled maintenance is started
/// Marks the beginning of the maintenance work
/// </summary>
public sealed record MaintenanceStarted : DomainEvent
{
    public AssetMaintenance? Maintenance { get; set; }
    public DateTime StartedOn { get; set; }
}
