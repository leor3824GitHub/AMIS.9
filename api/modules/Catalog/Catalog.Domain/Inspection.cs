using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class Inspection : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid? InspectorId { get; private set; } // Employee ID
    public Guid? InspectionRequestId { get; private set; }
    public DateTime InspectionDate { get; private set; }
    public InspectionStatus? InspectionStatus { get; private set; }
    public string? Remarks { get; private set; }

    // Navigation
    public virtual InspectionRequest? InspectionRequest { get; private set; } = default!;
    public virtual Purchase? Purchase { get; private set; } = default!;
    public virtual Employee? Inspector { get; private set; } = default!;
    public virtual ICollection<InspectionItem> Items { get; private set; } = [];

    // Optional helper: was partial inspection?
    public bool IsPartial => Items.Any(i => i.QtyFailed > 0);

    private Inspection() { }

    private Inspection(
        Guid id,
        Guid purchaseId,
        Guid inspectorId,
        Guid? inspectionRequestId,
        DateTime inspectionDate,
        InspectionStatus? inspectionStatus,
        string? remarks)
    {
        Id = id;
        PurchaseId = purchaseId;
        InspectorId = inspectorId;
        InspectionRequestId = inspectionRequestId;
        InspectionDate = inspectionDate;
        InspectionStatus = inspectionStatus;
        Remarks = remarks;

        QueueDomainEvent(new InspectionCreated { Inspection = this });
    }

    public static Inspection Create(
        Guid purchaseId,
        Guid inspectorId,
        Guid? inspectionRequestId,
        DateTime inspectionDate,
        InspectionStatus? inspectionStatus,
        string? remarks)
    {
        return new Inspection(
            Guid.NewGuid(),
            purchaseId,
            inspectorId,
            inspectionRequestId,
            inspectionDate,
            inspectionStatus,
            remarks);
    }

    public Inspection Update(
        Guid? inspectorId,
        Guid? inspectionRequestId,
        DateTime inspectionDate,
        InspectionStatus? inspectionStatus,
        string? remarks)
    {
        bool isUpdated = false;

        if (InspectorId != inspectorId)
        {
            InspectorId = inspectorId;
            isUpdated = true;
        }

        if (InspectionRequestId != inspectionRequestId)
        {
            InspectionRequestId = inspectionRequestId;
            isUpdated = true;
        }

        if (InspectionDate != inspectionDate)
        {
            InspectionDate = inspectionDate;
            isUpdated = true;
        }

        if (InspectionStatus != inspectionStatus)
        {
            InspectionStatus = inspectionStatus;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InspectionUpdated { Inspection = this });
        }

        return this;
    }

    public void AddItem(Guid purchaseItemId, int qtyInspected, int qtyPassed, int qtyFailed, string? remarks)
    {
        if (qtyPassed + qtyFailed != qtyInspected)
            throw new ArgumentException("The sum of passed and failed quantities must equal the inspected quantity.");

        var item = InspectionItem.Create(Id, purchaseItemId, qtyInspected, qtyPassed, qtyFailed, remarks);
        Items.Add(item);
    }

    public void RemoveItem(Guid purchaseItemId)
    {
        var item = Items.FirstOrDefault(x => x.PurchaseItemId == purchaseItemId);
        if (item is not null)
            Items.Remove(item);
    }
}
