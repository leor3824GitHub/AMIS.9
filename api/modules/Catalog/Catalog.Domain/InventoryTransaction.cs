using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class InventoryTransaction : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitCost { get; private set; }
    public string? Location { get; private set; } // Added Location property
    public Guid? SourceId { get; private set; }
    public TransactionType TransactionType { get; private set; } = TransactionType.Issuance;
    public virtual Product Product { get; private set; } = default!;

    private InventoryTransaction() { }

    private InventoryTransaction(Guid id, Guid? productId, int qty, decimal purchasePrice, string? location, Guid? sourceId, TransactionType transactionType)
    {
        Id = id;
        ProductId = productId;
        Qty = qty;
        UnitCost = purchasePrice;
        Location = location; // Initialize Location
        SourceId = sourceId;
        TransactionType = transactionType;
        QueueDomainEvent(new InventoryTransactionUpdated { InventoryTransaction = this });
    }

    public static InventoryTransaction Create(Guid? productId, int qty, decimal purchasePrice, string? location, Guid? sourceId, TransactionType transactionType)
    {
        ValidateStock(qty, purchasePrice);
        return new InventoryTransaction(Guid.NewGuid(), productId, qty, purchasePrice, location, sourceId, transactionType);
    }

    public InventoryTransaction Update(Guid? productId, int qty, decimal purchasePrice, string? location, Guid? sourceId, TransactionType transactionType)
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

        if (Location != location) // Update Location
        {
            Location = location;
            isUpdated = true;
        }

        if (SourceId != sourceId)
        {
            SourceId = sourceId;
            isUpdated = true;
        }

        if (TransactionType != transactionType)
        {
            TransactionType = transactionType;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryTransactionUpdated { InventoryTransaction = this });
        }

        return this;
    }

    private static void ValidateStock(int qty, decimal price)
    {
        if (qty <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (price <= 0) throw new ArgumentException("Price must be greater than zero.");
    }
}
