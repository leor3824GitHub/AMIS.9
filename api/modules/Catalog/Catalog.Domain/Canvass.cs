using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents a price quotation from a supplier for a specific purchase request.
/// Used to capture and compare supplier quotes during the canvassing process.
/// </summary>
public class Canvass : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseRequestId { get; private set; }
    public Guid SupplierId { get; private set; }
    public string ItemDescription { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public string Unit { get; private set; } = string.Empty;
    public decimal QuotedPrice { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime ResponseDate { get; private set; }
    public bool IsSelected { get; private set; }

    // Navigation properties
    public virtual PurchaseRequest? PurchaseRequest { get; init; }
    public virtual Supplier Supplier { get; private set; } = default!;

    private Canvass() { }

    private Canvass(
        Guid id,
        Guid purchaseRequestId,
        Guid supplierId,
        string itemDescription,
        int quantity,
        string unit,
        decimal quotedPrice,
        string? remarks,
        DateTime responseDate)
    {
        if (quotedPrice < 0)
            throw new ArgumentException("Quoted price cannot be negative.", nameof(quotedPrice));
        if (supplierId == Guid.Empty)
            throw new ArgumentException("Supplier ID cannot be empty.", nameof(supplierId));
        if (purchaseRequestId == Guid.Empty)
            throw new ArgumentException("Purchase request ID cannot be empty.", nameof(purchaseRequestId));
        if (string.IsNullOrWhiteSpace(itemDescription))
            throw new ArgumentException("Item description cannot be empty.", nameof(itemDescription));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty.", nameof(unit));

        Id = id;
        PurchaseRequestId = purchaseRequestId;
        SupplierId = supplierId;
        ItemDescription = itemDescription;
        Quantity = quantity;
        Unit = unit;
        QuotedPrice = quotedPrice;
        Remarks = remarks;
        ResponseDate = responseDate;
        IsSelected = false;

        QueueDomainEvent(new CanvassCreated { Canvass = this });
    }

    public static Canvass Create(
        Guid purchaseRequestId,
        Guid supplierId,
        string itemDescription,
        int quantity,
        string unit,
        decimal quotedPrice,
        string? remarks,
        DateTime responseDate)
    {
        return new Canvass(Guid.NewGuid(), purchaseRequestId, supplierId, itemDescription, quantity, unit, quotedPrice, remarks, responseDate);
    }

    public static Canvass Create(
        Guid id,
        Guid purchaseRequestId,
        Guid supplierId,
        string itemDescription,
        int quantity,
        string unit,
        decimal quotedPrice,
        string? remarks,
        DateTime responseDate)
    {
        return new Canvass(id, purchaseRequestId, supplierId, itemDescription, quantity, unit, quotedPrice, remarks, responseDate);
    }

    public Canvass Update(
        string itemDescription,
        int quantity,
        string unit,
        decimal quotedPrice,
        string? remarks,
        DateTime responseDate)
    {
        if (quotedPrice < 0)
            throw new ArgumentException("Quoted price cannot be negative.", nameof(quotedPrice));
        if (string.IsNullOrWhiteSpace(itemDescription))
            throw new ArgumentException("Item description cannot be empty.", nameof(itemDescription));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty.", nameof(unit));

        bool isUpdated = false;

        if (ItemDescription != itemDescription)
        {
            ItemDescription = itemDescription;
            isUpdated = true;
        }

        if (Quantity != quantity)
        {
            Quantity = quantity;
            isUpdated = true;
        }

        if (Unit != unit)
        {
            Unit = unit;
            isUpdated = true;
        }

        if (QuotedPrice != quotedPrice)
        {
            QuotedPrice = quotedPrice;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (ResponseDate != responseDate)
        {
            ResponseDate = responseDate;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CanvassUpdated { Canvass = this });
        }

        return this;
    }

    public void MarkAsSelected()
    {
        if (!IsSelected)
        {
            IsSelected = true;
            QueueDomainEvent(new CanvassUpdated { Canvass = this });
        }
    }

    public void UnmarkAsSelected()
    {
        if (IsSelected)
        {
            IsSelected = false;
            QueueDomainEvent(new CanvassUpdated { Canvass = this });
        }
    }
}
