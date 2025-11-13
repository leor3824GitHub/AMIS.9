using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class PurchaseItem : AuditableEntity
{
    public Guid PurchaseId { get; private set; }
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public PurchaseStatus ItemStatus { get; private set; }
    public PurchaseItemInspectionStatus InspectionStatus { get; private set; }
    public PurchaseItemAcceptanceStatus AcceptanceStatus { get; private set; }

    // Summary fields - computed from related items
    public int QtyInspected { get; private set; }
    public int QtyPassed { get; private set; }
    public int QtyFailed { get; private set; }
    public int QtyAccepted { get; private set; }

    // Computed properties
    public decimal TotalPrice => Qty * UnitPrice;
    public int QtyRemaining => Qty - QtyAccepted;
    public bool IsFullyInspected => QtyInspected >= Qty;
    public bool IsFullyAccepted => QtyAccepted >= Qty;
    public bool HasFailedInspection => QtyFailed > 0;

    // Navigation
    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Product? Product { get; private set; }
    public virtual ICollection<InspectionItem> InspectionItems { get; private set; } = [];
    public virtual ICollection<AcceptanceItem> AcceptanceItems { get; private set; } = [];

    private PurchaseItem() { }

    private PurchaseItem(Guid id, Guid purchaseId, Guid? productId, int qty, decimal unitPrice, PurchaseStatus itemStatus)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        Id = id;
        PurchaseId = purchaseId;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        ItemStatus = itemStatus;
        InspectionStatus = PurchaseItemInspectionStatus.NotInspected;
        AcceptanceStatus = PurchaseItemAcceptanceStatus.Pending;

        QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = this });
    }

    public static PurchaseItem Create(Guid purchaseId, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemStatus)
    {
        return new PurchaseItem(Guid.NewGuid(), purchaseId, productId, qty, unitPrice, itemStatus ?? PurchaseStatus.Draft);
    }

    public static PurchaseItem Create(Guid itemId, Guid purchaseId, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemStatus)
    {
        return new PurchaseItem(itemId, purchaseId, productId, qty, unitPrice, itemStatus ?? PurchaseStatus.Draft);
    }

    internal void SetPurchaseId(Guid purchaseId)
    {
        if (PurchaseId != Guid.Empty && PurchaseId != purchaseId)
        {
            throw new InvalidOperationException("PurchaseId is already set to a different value.");
        }
        PurchaseId = purchaseId;
    }

    public PurchaseItem Update(Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemStatus)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

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

        if (UnitPrice != unitPrice)
        {
            UnitPrice = unitPrice;
            isUpdated = true;
        }

        if (ItemStatus != (itemStatus ?? PurchaseStatus.Draft))
        {
            ItemStatus = itemStatus ?? PurchaseStatus.Draft;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = this });
        }

        return this;
    }

    public void UpdateInspectionSummary(int inspected, int passed, int failed)
    {
        if (inspected < 0 || passed < 0 || failed < 0)
            throw new ArgumentException("Quantities cannot be negative.");

        if (passed + failed != inspected)
            throw new ArgumentException("Passed + Failed must equal Inspected quantity.");

        QtyInspected = inspected;
        QtyPassed = passed;
        QtyFailed = failed;

        // Auto-update inspection status
        if (QtyInspected == 0)
        {
            UpdateInspectionStatus(PurchaseItemInspectionStatus.NotInspected);
        }
        else if (QtyFailed > 0 && QtyPassed == 0)
        {
            UpdateInspectionStatus(PurchaseItemInspectionStatus.Failed);
        }
        else if (QtyPassed > 0 && QtyFailed == 0 && QtyInspected >= Qty)
        {
            UpdateInspectionStatus(PurchaseItemInspectionStatus.Passed);
        }
        else if (QtyPassed > 0 && QtyInspected < Qty)
        {
            UpdateInspectionStatus(PurchaseItemInspectionStatus.PartiallyPassed);
        }
        else if (QtyPassed > 0 && QtyFailed > 0)
        {
            UpdateInspectionStatus(PurchaseItemInspectionStatus.PartiallyPassed);
        }
    }

    public void UpdateAcceptanceSummary(int accepted)
    {
        if (accepted < 0)
            throw new ArgumentException("Accepted quantity cannot be negative.", nameof(accepted));

        if (accepted > QtyPassed)
            throw new InvalidOperationException("Cannot accept more than passed quantity.");

        QtyAccepted = accepted;

        // Auto-update acceptance status
        if (QtyAccepted == 0)
        {
            UpdateAcceptanceStatus(PurchaseItemAcceptanceStatus.Pending);
        }
        else if (QtyAccepted >= Qty)
        {
            UpdateAcceptanceStatus(PurchaseItemAcceptanceStatus.Accepted);
        }
        else if (QtyAccepted > 0 && QtyAccepted < Qty)
        {
            UpdateAcceptanceStatus(PurchaseItemAcceptanceStatus.PartiallyAccepted);
        }
    }

    public PurchaseItem UpdateInspectionStatus(PurchaseItemInspectionStatus status)
    {
        if (InspectionStatus != status)
        {
            InspectionStatus = status;
            QueueDomainEvent(new PurchaseItemInspected { PurchaseItem = this });
        }

        return this;
    }

    public PurchaseItem UpdateAcceptanceStatus(PurchaseItemAcceptanceStatus status)
    {
        if (AcceptanceStatus != status)
        {
            AcceptanceStatus = status;
            QueueDomainEvent(new PurchaseItemAccepted { PurchaseItem = this });
        }

        return this;
    }
}

