using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when a disposal request is cancelled/rejected
/// Maintains audit trail of disposal decisions
/// </summary>
public sealed record DisposalCancelled : DomainEvent
{
    public AssetDisposal? Disposal { get; set; }
    public string? CancellationReason { get; set; }
}
