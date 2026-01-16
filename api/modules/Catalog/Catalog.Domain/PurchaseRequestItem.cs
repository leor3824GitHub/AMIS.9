using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class PurchaseRequestItem : AuditableEntity
{
    public Guid PurchaseRequestId { get; private set; }
    public Guid? ProductId { get; private set; }
    public string? ManualProductName { get; private set; }
    public int Qty { get; private set; }
    public string Unit { get; private set; } = "Piece";
    public string? Description { get; private set; }

    // Navigation
    public virtual PurchaseRequest PurchaseRequest { get; private set; } = default!;
    public virtual Product? Product { get; private set; }

    private PurchaseRequestItem() { }

    private PurchaseRequestItem(Guid id, Guid purchaseRequestId, Guid? productId, string? manualProductName, int qty, string unit, string? description)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be null or empty.", nameof(unit));

        var hasProductId = productId is not null;
        var hasManualName = !string.IsNullOrWhiteSpace(manualProductName);
        if (hasProductId == hasManualName)
            throw new ArgumentException("Exactly one of ProductId or ManualProductName must be provided.");

        Id = id;
        PurchaseRequestId = purchaseRequestId;
        ProductId = productId;
        ManualProductName = string.IsNullOrWhiteSpace(manualProductName) ? null : manualProductName.Trim();
        Qty = qty;
        Unit = unit;
        Description = description;

        QueueDomainEvent(new PurchaseRequestItemCreated { PurchaseRequestItem = this });
    }

    public static PurchaseRequestItem Create(Guid purchaseRequestId, Guid? productId, string? manualProductName, int qty, string unit, string? description)
    {
        return new PurchaseRequestItem(Guid.NewGuid(), purchaseRequestId, productId, manualProductName, qty, unit, description);
    }

    public static PurchaseRequestItem Create(Guid itemId, Guid purchaseRequestId, Guid? productId, string? manualProductName, int qty, string unit, string? description)
    {
        return new PurchaseRequestItem(itemId, purchaseRequestId, productId, manualProductName, qty, unit, description);
    }

    internal void SetPurchaseRequestId(Guid purchaseRequestId)
    {
        if (PurchaseRequestId != Guid.Empty && PurchaseRequestId != purchaseRequestId)
        {
            throw new InvalidOperationException("PurchaseRequestId is already set to a different value.");
        }
        PurchaseRequestId = purchaseRequestId;
    }

    public PurchaseRequestItem Update(Guid? productId, string? manualProductName, int qty, string unit, string? description)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be null or empty.", nameof(unit));

        var hasProductId = productId is not null;
        var hasManualName = !string.IsNullOrWhiteSpace(manualProductName);
        if (hasProductId == hasManualName)
            throw new ArgumentException("Exactly one of ProductId or ManualProductName must be provided.");

        bool isUpdated = false;

        var normalizedManualName = string.IsNullOrWhiteSpace(manualProductName) ? null : manualProductName.Trim();

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (ManualProductName != normalizedManualName)
        {
            ManualProductName = normalizedManualName;
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
