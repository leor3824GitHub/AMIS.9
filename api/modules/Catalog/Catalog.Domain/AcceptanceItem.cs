using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class AcceptanceItem : AuditableEntity
{
    public Guid AcceptanceId { get; private set; }
    public Guid PurchaseItemId { get; private set; }
    public int QtyAccepted { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Acceptance Acceptance { get; private set; } = default!;
    public virtual PurchaseItem PurchaseItem { get; private set; } = default!;

    // Computed properties
    public bool IsFullyAccepted => PurchaseItem != null && QtyAccepted >= PurchaseItem.Qty;
    public bool IsPartiallyAccepted => QtyAccepted > 0 && !IsFullyAccepted;
    public int QtyRemaining => PurchaseItem?.Qty - QtyAccepted ?? 0;

    private AcceptanceItem() { }

    private AcceptanceItem(Guid id, Guid acceptanceId, Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        if (qtyAccepted <= 0)
            throw new ArgumentException("Accepted quantity must be greater than zero.", nameof(qtyAccepted));

        Id = id;
        AcceptanceId = acceptanceId;
        PurchaseItemId = purchaseItemId;
        QtyAccepted = qtyAccepted;
        Remarks = remarks;

        QueueDomainEvent(new AcceptanceItemCreated { AcceptanceItem = this });
    }

    public static AcceptanceItem Create(Guid acceptanceId, Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        if (acceptanceId == Guid.Empty)
            throw new ArgumentException("AcceptanceId must be provided.", nameof(acceptanceId));
        
        if (purchaseItemId == Guid.Empty)
            throw new ArgumentException("PurchaseItemId must be provided.", nameof(purchaseItemId));

        return new AcceptanceItem(Guid.NewGuid(), acceptanceId, purchaseItemId, qtyAccepted, remarks);
    }

    public void Update(int qtyAccepted, string? remarks)
    {
        if (qtyAccepted <= 0)
            throw new ArgumentException("Accepted quantity must be greater than zero.", nameof(qtyAccepted));

        bool isUpdated = false;

        if (QtyAccepted != qtyAccepted)
        {
            QtyAccepted = qtyAccepted;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new AcceptanceItemUpdated { AcceptanceItem = this });
        }
    }

    public void ValidateAgainstPurchaseItem(PurchaseItem purchaseItem)
    {
        if (purchaseItem.Id != this.PurchaseItemId)
        {
            throw new InvalidOperationException("Purchase item mismatch.");
        }

        if (QtyAccepted > purchaseItem.Qty)
        {
            throw new InvalidOperationException($"Cannot accept more ({QtyAccepted}) than purchased ({purchaseItem.Qty}).");
        }
    }
}
