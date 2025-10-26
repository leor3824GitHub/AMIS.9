using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class Inspection : AuditableEntity, IAggregateRoot
{
    // Optional link to a Purchase
    public Guid? PurchaseId { get; private set; }
    public virtual Purchase? Purchase { get; private set; }

    // Employee who performed the inspection
    public Guid EmployeeId { get; private set; }
    public virtual Employee Employee { get; private set; } = default!;

    // Inspection details
    public DateTime InspectedOn { get; private set; }
    public bool Approved { get; private set; }
    public InspectionStatus Status { get; private set; }
    public string? Remarks { get; private set; }
    public string? IARDocumentPath { get; private set; }

    // Use InspectionItem for line items
    public virtual ICollection<InspectionItem> Items { get; private set; } = new List<InspectionItem>();

    // Parameterless ctor for EF
    private Inspection() { }

    // Internal constructor - use factory Create(...) for creation
    private Inspection(Guid id, Guid? purchaseId, Guid employeeId, DateTime inspectedOn, bool approved, string? remarks, string? iarDocumentPath)
    {
        Id = id;
        PurchaseId = purchaseId;
        EmployeeId = employeeId;
        InspectedOn = inspectedOn;
        Approved = approved;
        Remarks = remarks;
        IARDocumentPath = iarDocumentPath;
    Items = new List<InspectionItem>();
    Status = InspectionStatus.InProgress;
    }

    // Factory - ensures Id is generated and sensible defaults are applied
    public static Inspection Create(Guid? purchaseId, Guid employeeId, DateTime? inspectedOn = null, bool approved = false, string? remarks = null, string? iarDocumentPath = null)
    {
        if (employeeId == Guid.Empty) throw new ArgumentException("EmployeeId must be provided.", nameof(employeeId));
        var inspectedAt = inspectedOn ?? DateTime.UtcNow;
        var inspection = new Inspection(Guid.NewGuid(), purchaseId, employeeId, inspectedAt, approved, remarks, iarDocumentPath);

        // Queue domain event for creation
        inspection.QueueDomainEvent(new InspectionCreated
        {
            InspectionId = inspection.Id,
            PurchaseId = purchaseId,
            EmployeeId = employeeId
        });
        
        return inspection;
    }

    // Add an existing item instance (sets link to this inspection if missing)
    public void AddItem(InspectionItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // Ensure the item points to this inspection; use the domain Update method on the item
        if (item.InspectionId != this.Id)
        {
            item.Update(this.Id,
                        item.PurchaseItemId,
                        item.QtyInspected,
                        item.QtyPassed,
                        item.QtyFailed,
                        item.Remarks,
                        item.InspectionItemStatus);
        }

        Items.Add(item);
    }

    // Convenience overload: create and add item in one call
    public InspectionItem AddItem(Guid purchaseItemId, int qtyInspected, int qtyPassed, int qtyFailed, string? remarks = null, InspectionItemStatus? inspectionItemStatus = null)
    {
        var item = InspectionItem.Create(this.Id, purchaseItemId, qtyInspected, qtyPassed, qtyFailed, remarks, inspectionItemStatus);
        Items.Add(item);
        return item;
    }

    public void RemoveItem(InspectionItem item)
    {
    ArgumentNullException.ThrowIfNull(item);
        if (Items.Remove(item))
        {
            // detach navigation - keep InspectionId as-is for audit/history; do not set to Guid.Empty to avoid losing historic link
        }
    }

    // Approve the inspection and emit domain event
    public void Approve()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException("Cannot approve an inspection without items.");

        // Business invariant: at least one accepted/passed item to consider approval meaningful
        if (!Items.Any(i => i.InspectionItemStatus == InspectionItemStatus.Passed ||
                            i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation))
            throw new InvalidOperationException("Cannot approve an inspection where no items are passed/accepted.");

        if (Status == InspectionStatus.InProgress)
        {
            ChangeStatus(InspectionStatus.Completed);
        }

        ChangeStatus(InspectionStatus.Approved);
        Approved = true;

        // Queue domain event for handlers to react (inventory updates, accounting, etc.)
        QueueDomainEvent(new InspectionApproved
        {
            InspectionId = this.Id,
            PurchaseId = this.PurchaseId,
            EmployeeId = this.EmployeeId,
            ApprovedOn = DateTime.UtcNow
        });
    }

    // Reject the inspection and emit domain event
    public void Reject(string? reason = null)
    {
        ChangeStatus(InspectionStatus.Rejected);
        Approved = false;
        QueueDomainEvent(new InspectionRejected
        {
            InspectionId = this.Id,
            PurchaseId = this.PurchaseId,
            EmployeeId = this.EmployeeId,
            RejectedOn = DateTime.UtcNow,
            Reason = reason
        });
    }

    public void UpdateRemarks(string? remarks)
    {
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks;
    }

    public void UpdateIARDocument(string? path)
    {
        IARDocumentPath = string.IsNullOrWhiteSpace(path) ? null : path;
    }

    // Convenience queries
    public IEnumerable<InspectionItem> AcceptedItems() => Items.Where(i => i.InspectionItemStatus == InspectionItemStatus.Passed || i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation).ToList();

    public int TotalInspectedQuantity() => Items.Sum(i => i.QtyInspected);

    public int TotalAcceptedQuantity() => Items.Where(i => i.InspectionItemStatus == InspectionItemStatus.Passed || i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation)
                                              .Sum(i => i.QtyPassed);

    // Set inspection date (with validation)
    public void SetInspectedOn(DateTime inspectedOn)
    {
        if (inspectedOn == default)
            throw new ArgumentException("InspectedOn must be a valid date.", nameof(inspectedOn));
        InspectedOn = inspectedOn;
    }

    // Update the employee (inspector) if needed
    public void SetEmployee(Guid employeeId)
    {
        if (employeeId == Guid.Empty) throw new ArgumentException("EmployeeId must be provided.", nameof(employeeId));
        if (EmployeeId == employeeId) return;
        EmployeeId = employeeId;
        Employee = null!;
    }

    // Update purchase link if needed
    public void SetPurchase(Guid? purchaseId)
    {
        PurchaseId = purchaseId;
    }

    public void ChangeStatus(InspectionStatus newStatus)
    {
        if (Status == newStatus)
        {
            return;
        }

        if (!IsValidTransition(Status, newStatus))
        {
            throw new InvalidOperationException($"Cannot transition inspection from {Status} to {newStatus}.");
        }

        Status = newStatus;
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    public void Complete()
    {
        ChangeStatus(InspectionStatus.Completed);
    }

    private static bool IsValidTransition(InspectionStatus current, InspectionStatus next)
    {
        var validTransitions = new Dictionary<InspectionStatus, InspectionStatus[]>
        {
            { InspectionStatus.InProgress, new[] { InspectionStatus.Completed, InspectionStatus.Approved, InspectionStatus.Rejected, InspectionStatus.Cancelled } },
            { InspectionStatus.Completed, new[] { InspectionStatus.Approved, InspectionStatus.Rejected } },
            { InspectionStatus.Approved, Array.Empty<InspectionStatus>() },
            { InspectionStatus.Rejected, Array.Empty<InspectionStatus>() },
            { InspectionStatus.Cancelled, Array.Empty<InspectionStatus>() }
        };

        return validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}
