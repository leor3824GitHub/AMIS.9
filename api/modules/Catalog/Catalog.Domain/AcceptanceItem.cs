using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
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

    private AcceptanceItem() { }

    private AcceptanceItem(Guid id, Guid acceptanceId, Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        Id = id;
        AcceptanceId = acceptanceId;
        PurchaseItemId = purchaseItemId;
        QtyAccepted = qtyAccepted;
        Remarks = remarks;

        QueueDomainEvent(new AcceptanceItemCreated { AcceptanceItem = this });
    }

    public static AcceptanceItem Create(Guid acceptanceId, Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        return new AcceptanceItem(Guid.NewGuid(), acceptanceId, purchaseItemId, qtyAccepted, remarks);
    }

    public AcceptanceItem Update(Guid acceptanceId, Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        bool isUpdated = false;

        if (AcceptanceId != acceptanceId)
        {
            AcceptanceId = acceptanceId;
            isUpdated = true;
        }

        if (PurchaseItemId != purchaseItemId)
        {
            PurchaseItemId = purchaseItemId;
            isUpdated = true;
        }

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

        return this;
    }

}
