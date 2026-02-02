using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when maintenance is scheduled for an asset
/// Initiates maintenance record in the system
/// </summary>
public sealed record MaintenanceScheduled : DomainEvent
{
    public AssetMaintenance? Maintenance { get; set; }
}
