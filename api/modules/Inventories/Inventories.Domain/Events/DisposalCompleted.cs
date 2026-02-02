using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

/// <summary>
/// Domain event raised when an asset disposal is completed
/// Asset is removed from inventory and financial impact is recorded
/// Critical for COA audit compliance
/// </summary>
public sealed record DisposalCompleted : DomainEvent
{
    public AssetDisposal? Disposal { get; set; }
    public DateTime CompletedOn { get; set; }
    public decimal? GainOrLoss { get; set; } // Positive = Gain, Negative = Loss
}
