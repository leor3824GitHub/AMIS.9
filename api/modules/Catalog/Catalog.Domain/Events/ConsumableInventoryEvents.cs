using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

// Consumable Inventory Events
public sealed record ConsumableInventoryCreated : DomainEvent
{
    public ConsumableInventory ConsumableInventory { get; set; } = default!;
}

public sealed record ConsumableInventoryUpdated : DomainEvent
{
    public ConsumableInventory ConsumableInventory { get; set; } = default!;
}

public sealed record ConsumableStockReceived : DomainEvent
{
    public ConsumableInventory ConsumableInventory { get; set; } = default!;
    public int QuantityReceived { get; set; }
    public decimal UnitCost { get; set; }
    public string? PONumber { get; set; }
}

public sealed record ConsumableStockIssued : DomainEvent
{
    public ConsumableInventory ConsumableInventory { get; set; } = default!;
    public int QuantityIssued { get; set; }
    public Guid IssuedTo { get; set; }
    public string? RSMINumber { get; set; }
    public decimal UnitCost { get; set; }
}

public sealed record ConsumableStockAdjusted : DomainEvent
{
    public ConsumableInventory ConsumableInventory { get; set; } = default!;
    public int Adjustment { get; set; }
    public string Reason { get; set; } = default!;
}
