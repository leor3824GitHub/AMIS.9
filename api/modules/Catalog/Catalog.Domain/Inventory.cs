using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Inventory : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public string? Location { get; private set; }
    public decimal Qty { get; private set; }
    public decimal AvePrice { get; private set; }
    public string? Unit { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private Inventory() { }

    private Inventory(Guid id, Guid? productId, string? location, decimal qty, decimal avePrice, string? unit)
    {
        Id = id;
        ProductId = productId;
        Location = location;
        Qty = qty;
        AvePrice = avePrice;
        Unit = unit;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public static Inventory Create(Guid? productId, string? location, decimal qty, decimal avePrice, string? unit)
    {
        return new Inventory(Guid.NewGuid(), productId, location, qty, avePrice, unit);
    }

    public Inventory Update(Guid? productId, string? location, decimal qty, decimal avePrice, string? unit)
    {
        bool isUpdated = false;

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (!string.Equals(Location, location, StringComparison.OrdinalIgnoreCase))
        {
            Location = location;
            isUpdated = true;
        }

        if (Qty != qty)
        {
            Qty = qty;
            isUpdated = true;
        }

        if (AvePrice != avePrice)
        {
            AvePrice = avePrice;
            isUpdated = true;
        }

        if (!string.Equals(Unit, unit, StringComparison.OrdinalIgnoreCase))
        {
            Unit = unit;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryUpdated { Inventory = this });
        }

        return this;
    }

    public void AddStock(int qty, decimal unitPrice)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");

        // Update Average Price and Quantity for this specific inventory record
        AvePrice = ((AvePrice * Qty) + (unitPrice * qty)) / (Qty + qty);
        Qty += qty;
    }

    public void RemoveStock(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (Qty < qty) throw new InvalidOperationException("Not enough stock available.");

        // Reduce quantity for this inventory record
        Qty -= qty;
    }
}

