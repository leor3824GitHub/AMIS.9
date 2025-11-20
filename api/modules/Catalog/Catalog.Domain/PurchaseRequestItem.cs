using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class PurchaseRequestItem : AuditableEntity
{
    public Guid PurchaseRequestId { get; private set; }
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public string Unit { get; private set; } = "Piece";
    public string? Description { get; private set; }

    // Navigation
    public virtual PurchaseRequest PurchaseRequest { get; private set; } = default!;
    public virtual Product? Product { get; private set; }

    private PurchaseRequestItem() { }

    private PurchaseRequestItem(Guid id, Guid purchaseRequestId, Guid? productId, int qty, string unit, string? description)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be null or empty.", nameof(unit));

        Id = id;
        PurchaseRequestId = purchaseRequestId;
        ProductId = productId;
        Qty = qty;
        Unit = unit;
        Description = description;

        QueueDomainEvent(new PurchaseRequestItemCreated { PurchaseRequestItem = this });
    }

    public static PurchaseRequestItem Create(Guid purchaseRequestId, Guid? productId, int qty, string unit, string? description)
    {
        return new PurchaseRequestItem(Guid.NewGuid(), purchaseRequestId, productId, qty, unit, description);
    }

    public static PurchaseRequestItem Create(Guid itemId, Guid purchaseRequestId, Guid? productId, int qty, string unit, string? description)
    {
        return new PurchaseRequestItem(itemId, purchaseRequestId, productId, qty, unit, description);
    }

    internal void SetPurchaseRequestId(Guid purchaseRequestId)
    {
        if (PurchaseRequestId != Guid.Empty && PurchaseRequestId != purchaseRequestId)
        {
            throw new InvalidOperationException("PurchaseRequestId is already set to a different value.");
        }
        PurchaseRequestId = purchaseRequestId;
    }

    public PurchaseRequestItem Update(Guid? productId, int qty, string unit, string? description)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be null or empty.", nameof(unit));

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

        if (Unit != unit)
        {
            Unit = unit;
            isUpdated = true;
        }

        if (Description != description)
        {
            Description = description;
            isUpdated = true;
        }


        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseRequestItemUpdated { PurchaseRequestItem = this });
        }

        return this;
    }
}
