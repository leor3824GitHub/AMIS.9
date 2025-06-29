using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class InspectionItem : AuditableEntity, IAggregateRoot
{
    public Guid InspectionId { get; private set; }
    public Guid PurchaseItemId { get; private set; }
    public int QtyInspected { get; private set; }
    public int QtyPassed { get; private set; }
    public int QtyFailed { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Inspection? Inspection { get; private set; } = default!;
    public virtual PurchaseItem? PurchaseItem { get; private set; } = default!;

    private InspectionItem() { }

    private InspectionItem(Guid id, Guid inspectionId, Guid purchaseItemId, int qtyInspected, int qtyPassed, int qtyFailed, string? remarks)
    {
        Id = id;
        InspectionId = inspectionId;
        PurchaseItemId = purchaseItemId;
        QtyInspected = qtyInspected;
        QtyPassed = qtyPassed;
        QtyFailed = qtyFailed;
        Remarks = remarks;

        QueueDomainEvent(new InspectionItemCreated { InspectionItem = this });
    }

    public static InspectionItem Create(Guid inspectionId, Guid purchaseItemId, int qtyInspected, int qtyPassed, int qtyFailed, string? remarks)
    {
        return new InspectionItem(Guid.NewGuid(), inspectionId, purchaseItemId, qtyInspected, qtyPassed, qtyFailed, remarks);
    }

    public InspectionItem Update(
    Guid inspectionId,
    Guid purchaseItemId,
    int quantityInspected,
    int quantityPassed,
    int quantityFailed,
    string? remarks)
    {
        bool isUpdated = false;

        if (InspectionId != inspectionId)
        {
            InspectionId = inspectionId;
            isUpdated = true;
        }

        if (PurchaseItemId != purchaseItemId)
        {
            PurchaseItemId = purchaseItemId;
            isUpdated = true;
        }

        if (QtyInspected != quantityInspected)
        {
            QtyInspected = quantityInspected;
            isUpdated = true;
        }

        if (QtyPassed != quantityPassed)
        {
            QtyPassed = quantityPassed;
            isUpdated = true;
        }

        if (QtyFailed != quantityFailed)
        {
            QtyFailed = quantityFailed;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InspectionItemUpdated { InspectionItem = this });
        }

        return this;
    }

}
