using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when maintenance work is completed
/// Records final status and results of maintenance activity
/// </summary>
public sealed record MaintenanceCompleted : DomainEvent
{
    public AssetMaintenance? Maintenance { get; set; }
    public DateTime CompletedOn { get; set; }
}
