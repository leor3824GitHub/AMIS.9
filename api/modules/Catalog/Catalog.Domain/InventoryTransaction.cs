using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class InventoryTransaction : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitCost { get; private set; }
    public string Transaction_Type { get; private set; } = default!;
    public virtual Product Product { get; private set; } = default!;

    private InventoryTransaction() { }

    private InventoryTransaction(Guid id, Guid? productId, int qty, decimal purchasePrice, string transactionType)
    {
        Id = id;
        ProductId = productId;
        Qty = qty;
        UnitCost = purchasePrice;
        Transaction_Type = transactionType;
        QueueDomainEvent(new InventoryTransactionUpdated { InventoryTransaction = this });
    }

    public static InventoryTransaction Create(Guid? productId, int qty, decimal purchasePrice, string transactionType)
    {
        ValidateStock(qty, purchasePrice);
        return new InventoryTransaction(Guid.NewGuid(), productId, qty, purchasePrice, transactionType);
    }

    public InventoryTransaction Update(Guid? productId, int qty, decimal purchasePrice, string transactionType)
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

        if (UnitCost != purchasePrice)
        {
            UnitCost = purchasePrice;
            isUpdated = true;
        }

        if (Transaction_Type != transactionType)
        {
            Transaction_Type = transactionType;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryTransactionUpdated { InventoryTransaction = this });
        }

        return this;
    }

    // You can optionally allow updating the transaction type with AddStock, DeductStock, etc., if needed.
    // Just add a string transactionType parameter and assign it like:
    // Transaction_Type = transactionType;

    // Other methods remain unchanged unless you want to track or modify Transaction_Type in them.

    // ... [rest of the code remains unchanged] ...

    private static void ValidateStock(int qty, decimal price)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (price <= 0) throw new ArgumentException("Price must be greater than zero.");
    }
}
