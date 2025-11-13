using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class InspectionItem : AuditableEntity
{
    public Guid InspectionId { get; private set; }
    public Guid PurchaseItemId { get; private set; }
    public int QtyInspected { get; private set; }
    public int QtyPassed { get; private set; }
    public int QtyFailed { get; private set; }
    public string? Remarks { get; private set; }
    public InspectionItemStatus InspectionItemStatus { get; private set; }

    // Computed properties
    public decimal PassRate => QtyInspected > 0 ? (decimal)QtyPassed / QtyInspected * 100 : 0;
    public bool IsFullyPassed => QtyInspected > 0 && QtyFailed == 0;
    public bool IsFullyFailed => QtyInspected > 0 && QtyPassed == 0;

    public virtual Inspection Inspection { get; private set; } = default!;
    public virtual PurchaseItem PurchaseItem { get; private set; } = default!;

    private InspectionItem() { }

    private InspectionItem(
        Guid id,
        Guid inspectionId,
        Guid purchaseItemId,
        int qtyInspected,
        int qtyPassed,
        int qtyFailed,
        string? remarks,
        InspectionItemStatus? inspectionItemStatus)
    {
        ValidateQuantities(qtyInspected, qtyPassed, qtyFailed);

        Id = id;
        InspectionId = inspectionId;
        PurchaseItemId = purchaseItemId;
        QtyInspected = qtyInspected;
        QtyPassed = qtyPassed;
        QtyFailed = qtyFailed;
        Remarks = remarks;
        InspectionItemStatus = inspectionItemStatus ?? DetermineStatus(qtyPassed, qtyFailed, qtyInspected);

        QueueDomainEvent(new InspectionItemCreated { InspectionItem = this });
    }

    public static InspectionItem Create(
        Guid inspectionId,
        Guid purchaseItemId,
        int qtyInspected,
        int qtyPassed,
        int qtyFailed,
        string? remarks,
        InspectionItemStatus? inspectionItemStatus)
    {
        return new InspectionItem(
            Guid.NewGuid(),
            inspectionId,
            purchaseItemId,
            qtyInspected,
            qtyPassed,
            qtyFailed,
            remarks,
            inspectionItemStatus);
    }

    public InspectionItem Update(
        Guid inspectionId,
        Guid purchaseItemId,
        int quantityInspected,
        int quantityPassed,
        int quantityFailed,
        string? remarks,
        InspectionItemStatus? inspectionItemStatus)
    {
        ValidateQuantities(quantityInspected, quantityPassed, quantityFailed);

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

        var newStatus = inspectionItemStatus ?? DetermineStatus(quantityPassed, quantityFailed, quantityInspected);
        if (InspectionItemStatus != newStatus)
        {
            InspectionItemStatus = newStatus;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InspectionItemUpdated { InspectionItem = this });
        }

        return this;
    }

    private static void ValidateQuantities(int inspected, int passed, int failed)
    {
        if (inspected <= 0)
            throw new ArgumentException("Inspected quantity must be greater than zero.", nameof(inspected));
        
        if (passed < 0)
            throw new ArgumentException("Passed quantity cannot be negative.", nameof(passed));
        
        if (failed < 0)
            throw new ArgumentException("Failed quantity cannot be negative.", nameof(failed));
        
        if (passed + failed != inspected)
            throw new ArgumentException("Passed + Failed quantities must equal Inspected quantity.");
    }

    private static InspectionItemStatus DetermineStatus(int passed, int failed, int inspected)
    {
        if (inspected == 0)
            return InspectionItemStatus.NotInspected;
        
        if (passed == 0 && failed > 0)
            return InspectionItemStatus.Failed;
        
        if (failed == 0 && passed > 0)
            return InspectionItemStatus.Passed;
        
        if (passed > 0 && failed > 0)
            return InspectionItemStatus.Partial;
        
        return InspectionItemStatus.NotInspected;
    }
}
