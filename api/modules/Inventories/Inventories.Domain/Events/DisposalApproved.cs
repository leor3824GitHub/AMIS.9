using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when an asset disposal request is approved
/// Authorizes the disposal to proceed
/// Compliance requirement for asset retirement
/// </summary>
public sealed record DisposalApproved : DomainEvent
{
    public AssetDisposal? Disposal { get; set; }
    public Guid ApprovedBy { get; set; }
    public DateTime ApprovedOn { get; set; }
}
