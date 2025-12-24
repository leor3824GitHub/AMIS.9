using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

// Semi-Expendable Inventory Events
public sealed record SemiExpendableInventoryCreated : DomainEvent
{
    public SemiExpendableInventory SemiExpendableInventory { get; set; } = default!;
}

public sealed record SemiExpendableInventoryUpdated : DomainEvent
{
    public SemiExpendableInventory SemiExpendableInventory { get; set; } = default!;
}

public sealed record SemiExpendableStockReceived : DomainEvent
{
    public SemiExpendableInventory SemiExpendableInventory { get; set; } = default!;
    public int QuantityReceived { get; set; }
    public decimal UnitCost { get; set; }
    public string? PONumber { get; set; }
}

public sealed record SemiExpendableStockIssued : DomainEvent
{
    public SemiExpendableInventory SemiExpendableInventory { get; set; } = default!;
    public int QuantityIssued { get; set; }
    public Guid CustodianId { get; set; }
    public string? ICSNumber { get; set; }
    public decimal UnitCost { get; set; }
}

public sealed record SemiExpendableStockReturned : DomainEvent
{
    public SemiExpendableInventory SemiExpendableInventory { get; set; } = default!;
    public int QuantityReturned { get; set; }
    public string? Reason { get; set; }
}

public sealed record SemiExpendableStockAdjusted : DomainEvent
{
    public SemiExpendableInventory SemiExpendableInventory { get; set; } = default!;
    public int Adjustment { get; set; }
    public string Reason { get; set; } = default!;
}
