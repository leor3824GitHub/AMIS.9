using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when an asset disposal request is created
/// Initiates the audit trail for asset retirement
/// </summary>
public sealed record DisposalRequested : DomainEvent
{
    public AssetDisposal? Disposal { get; set; }
}
