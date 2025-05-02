using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class AcceptanceItem : AuditableEntity
{
    public Guid AcceptanceId { get; private set; }
    public Guid PurchaseItemId { get; private set; }
    public int QuantityAccepted { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Acceptance Acceptance { get; private set; } = default!;
    public virtual PurchaseItem PurchaseItem { get; private set; } = default!;

    private AcceptanceItem() { }

    private AcceptanceItem(Guid id, Guid acceptanceId, Guid purchaseItemId, int quantityAccepted, string? remarks)
    {
        Id = id;
        AcceptanceId = acceptanceId;
        PurchaseItemId = purchaseItemId;
        QuantityAccepted = quantityAccepted;
        Remarks = remarks;

        QueueDomainEvent(new AcceptanceItemCreated { AcceptanceItem = this });
    }

    public static AcceptanceItem Create(Guid acceptanceId, Guid purchaseItemId, int quantityAccepted, string? remarks)
    {
        return new AcceptanceItem(Guid.NewGuid(), acceptanceId, purchaseItemId, quantityAccepted, remarks);
    }

    public AcceptanceItem Update(int quantityAccepted, string? remarks)
    {
        bool isUpdated = false;

        if (QuantityAccepted != quantityAccepted)
        {
            QuantityAccepted = quantityAccepted;
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
