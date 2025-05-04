﻿using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class Inspection : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid InspectorId { get; private set; } // Employee ID
    public DateTime InspectionDate { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Employee Inspector { get; private set; } = default!;
    public virtual ICollection<InspectionItem> Items { get; private set; } = [];

    private Inspection() { }

    private Inspection(Guid id, Guid purchaseId, Guid inspectedId, DateTime inspectionDate, string? remarks)
    {
        Id = id;
        PurchaseId = purchaseId;
        InspectorId = inspectedId;
        InspectionDate = inspectionDate;
        Remarks = remarks;

        QueueDomainEvent(new InspectionCreated { Inspection = this });
    }

    public static Inspection Create(Guid purchaseId, Guid inspectedId, DateTime inspectionDate, string? remarks)
    {
        return new Inspection(Guid.NewGuid(), purchaseId, inspectedId, inspectionDate, remarks);
    }

    public Inspection Update(Guid inspectedId, DateTime inspectionDate, string? remarks)
    {
        bool isUpdated = false;

        if (InspectorId != inspectedId)
        {
            InspectorId = inspectedId;
            isUpdated = true;
        }

        if (InspectionDate != inspectionDate)
        {
            InspectionDate = inspectionDate;
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
        var item = InspectionItem.Create(Id, purchaseItemId, qtyInspected, qtyPassed, qtyFailed, remarks);
        Items.Add(item);
    }
}
