using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Inventory : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal AvePrice { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private Inventory() { }

    private Inventory(Guid id, Guid? productId, int qty, decimal purchasePrice)
    {
        Id = id;
        ProductId = productId;
        Qty = qty;
        AvePrice = purchasePrice;
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public static Inventory Create(Guid? productId, int qty, decimal purchasePrice)
    {
        ValidateStock(qty, purchasePrice);
        return new Inventory(Guid.NewGuid(), productId, qty, purchasePrice);
    }

    public Inventory Update(Guid? productId, int qty, decimal purchasePrice)
    {
        ValidateStock(qty, purchasePrice);

        bool isUpdated = false;

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (Qty != qty)
        {
            Qty = qty;
            isUpdated = true;
        }

        if (AvePrice != purchasePrice)
        {
            AvePrice = purchasePrice;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryUpdated { Inventory = this });
        }

        return this;
    }

    public void AddStock(int qty, decimal purchasePrice)
    {
        ValidateStock(qty, purchasePrice);

        // Update the average price
        AvePrice = ((AvePrice * Qty) + (purchasePrice * qty)) / (Qty + qty);
        Qty += qty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public void UpdateStock(int oldQty, int newQty, decimal purchasePrice)
    {
        ValidateStock(newQty, purchasePrice);
        if (oldQty < 0) throw new ArgumentException("Old quantity must be zero or greater.");

        int totalQty = Qty - oldQty + newQty;
        AvePrice = totalQty > 0 ? ((AvePrice * Qty) - (AvePrice * oldQty) + (purchasePrice * newQty)) / totalQty : 0;
        Qty = totalQty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public void UpdateStock(int qty)
    {
        if (qty < 0) throw new ArgumentException("Quantity must be zero or greater.");

        Qty += qty;
        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public void DeductStock(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (Qty < qty) throw new InvalidOperationException("Not enough stock available.");

        Qty -= qty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    public void DeductStock(int qty, decimal unitPrice)
    {
        ValidateStock(qty, unitPrice);

        if (Qty < qty) throw new InvalidOperationException("Not enough stock available.");

        int totalQty = Qty - qty;
        AvePrice = totalQty > 0 ? ((AvePrice * Qty) - (unitPrice * qty)) / totalQty : 0;
        Qty = totalQty;

        QueueDomainEvent(new InventoryUpdated { Inventory = this });
    }

    private static void ValidateStock(int qty, decimal price)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (price <= 0) throw new ArgumentException("Price must be greater than zero.");
    }
}