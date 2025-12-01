using AMIS.Framework.Core.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain.Events;

public record PhysicalAssetCreated : DomainEvent
{
    public PhysicalAsset PhysicalAsset { get; init; } = default!;
}

public record PhysicalAssetIssued : DomainEvent
{
    public PhysicalAsset PhysicalAsset { get; init; } = default!;
    public Guid EmployeeId { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public DocumentType DocumentType { get; init; }
    public int Quantity { get; init; }
}

public record PhysicalAssetReturned : DomainEvent
{
    public PhysicalAsset PhysicalAsset { get; init; } = default!;
    public string Reason { get; init; } = default!;
    public string Condition { get; init; } = default!;
    public int QuantityReturned { get; init; }
}

public record PhysicalAssetReclassified : DomainEvent
{
    public PhysicalAsset PhysicalAsset { get; init; } = default!;
    public PropertyClassification OldClassification { get; init; }
    public PropertyClassification NewClassification { get; init; }
    public string Reason { get; init; } = default!;
    public DateTime EffectiveDate { get; init; }
}

public record PhysicalAssetDepreciated : DomainEvent
{
    public PhysicalAsset PhysicalAsset { get; init; } = default!;
    public decimal Amount { get; init; }
    public DateTime DepreciationDate { get; init; }
    public decimal AccumulatedDepreciation { get; init; }
    public decimal BookValue { get; init; }
}

public record PhysicalAssetConditionUpdated : DomainEvent
{
    public PhysicalAsset PhysicalAsset { get; init; } = default!;
    public string Condition { get; init; } = default!;
    public string? Remarks { get; init; }
}
