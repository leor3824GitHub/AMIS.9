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
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (purchasePrice <= 0) throw new ArgumentException("Purchase price must be greater than zero.");

        return new Inventory(Guid.NewGuid(), productId, qty, purchasePrice);
    }

    public Inventory Update(Guid? productId, int qty, decimal purchasePrice)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (purchasePrice <= 0) throw new ArgumentException("Purchase price must be greater than zero.");

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
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        // Calculate the average price
        AvePrice = ((AvePrice * Qty) + (purchasePrice * qty)) / (Qty + qty);
        Qty += qty;
    }

    public void UpdateStock(int oldQty, int newQty, decimal purchasePrice)
    {
        if (oldQty < 0) throw new ArgumentException("Old quantity must be zero or greater.");
        if (newQty <= 0) throw new ArgumentException("New quantity must be greater than zero.");
        if (purchasePrice <= 0) throw new ArgumentException("Purchase price must be greater than zero.");

        // Calculate the total quantity and average price
        int totalQty = Qty - oldQty + newQty;
        AvePrice = ((AvePrice * Qty) - (AvePrice * oldQty) + (purchasePrice * newQty)) / totalQty;
        Qty = totalQty;
    }

    public void UpdateStock(int qty)
    {
        if (qty < 0) throw new ArgumentException("Old quantity must be zero or greater.");

        // add stock from deleted issuance item
        Qty += qty;
    }

    public void DeductStock(int qty)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (Qty < qty) throw new InvalidOperationException("Not enough stock available.");

        // Reduce quantity for this inventory record
        Qty -= qty;
    }
}

