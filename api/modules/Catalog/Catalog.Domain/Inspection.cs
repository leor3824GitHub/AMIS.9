using System;
using System.Collections.Generic;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;
public class Inspection : AuditableEntity, IAggregateRoot
{
    // Optional link to a Purchase
    public Guid? PurchaseId { get; set; }
    public virtual Purchase? Purchase { get; set; }

    // Employee who performed the inspection
    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = default!;

    // Inspection details
    public DateTime InspectedOn { get; set; }
    public bool Approved { get; set; }
    public string? Remarks { get; set; }
    public string? IARDocumentPath { get; set; }

    // Use InspectionItem instead of IARItem
    public virtual ICollection<InspectionItem> Items { get; set; } = new List<InspectionItem>();

    // Constructor
    public Inspection(
        Guid id,
        Guid? purchaseId,
        Guid employeeId,
        DateTime inspectedOn,
        bool approved,
        string? remarks,
        string? iarDocumentPath)
    {
        Id = id;
        PurchaseId = purchaseId;
        Purchase = null;
        EmployeeId = employeeId;
        Employee = default!;
        InspectedOn = inspectedOn;
        Approved = approved;
        Remarks = remarks;
        IARDocumentPath = iarDocumentPath;
        Items = new List<InspectionItem>();
    }

    // Domain methods
    public void MarkAsApproved()
    {
        Approved = true;
    }

    public void MarkAsRejected()
    {
        Approved = false;
    }

    public void AddItem(InspectionItem item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        Items.Add(item);
    }

    public void RemoveItem(InspectionItem item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        Items.Remove(item);
    }
}
