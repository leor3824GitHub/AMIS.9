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

    // Computed properties
    public bool HasItems => Items.Count > 0;
    public int TotalInspectedQuantity => Items.Sum(i => i.QtyInspected);
    public int TotalPassedQuantity => Items.Sum(i => i.QtyPassed);
    public int TotalFailedQuantity => Items.Sum(i => i.QtyFailed);
    public bool HasAnyPassed => Items.Any(i => 
        i.InspectionItemStatus == InspectionItemStatus.Passed || 
        i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation);
    public bool HasAnyFailed => Items.Any(i => 
        i.InspectionItemStatus == InspectionItemStatus.Failed || 
        i.InspectionItemStatus == InspectionItemStatus.Rejected);

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
    public static Inspection Create(Guid? purchaseId, Guid employeeId, DateTime? inspectedOn = null, string? remarks = null, string? iarDocumentPath = null)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("EmployeeId must be provided.", nameof(employeeId));

        var inspectedAt = inspectedOn ?? DateTime.UtcNow;
        var inspection = new Inspection(Guid.NewGuid(), purchaseId, employeeId, inspectedAt, false, remarks, iarDocumentPath);

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

        if (Status != InspectionStatus.InProgress)
        {
            throw new InvalidOperationException($"Cannot add items to an inspection with status {Status}.");
        }

        // Ensure the item points to this inspection
        if (item.InspectionId != this.Id)
        {
            item.Update(this.Id, item.PurchaseItemId, item.QtyInspected, item.QtyPassed, item.QtyFailed, item.Remarks, item.InspectionItemStatus);
        }

        // Check if item already exists for this purchase item
        if (Items.Any(i => i.PurchaseItemId == item.PurchaseItemId && i.Id != item.Id))
        {
            throw new InvalidOperationException($"Inspection item for purchase item {item.PurchaseItemId} already exists.");
        }

        Items.Add(item);
    }

    // Convenience overload: create and add item in one call
    public InspectionItem AddItem(Guid purchaseItemId, int qtyInspected, int qtyPassed, int qtyFailed, string? remarks = null, InspectionItemStatus? inspectionItemStatus = null)
    {
        if (Status != InspectionStatus.InProgress)
        {
            throw new InvalidOperationException($"Cannot add items to an inspection with status {Status}.");
        }

        if (qtyInspected <= 0)
        {
            throw new ArgumentException("Inspected quantity must be greater than zero.", nameof(qtyInspected));
        }

        if (qtyPassed + qtyFailed != qtyInspected)
        {
            throw new ArgumentException("Passed + Failed quantities must equal Inspected quantity.");
        }

        // Check if item already exists for this purchase item
        if (Items.Any(i => i.PurchaseItemId == purchaseItemId))
        {
            throw new InvalidOperationException($"Inspection item for purchase item {purchaseItemId} already exists.");
        }

        var item = InspectionItem.Create(this.Id, purchaseItemId, qtyInspected, qtyPassed, qtyFailed, remarks, inspectionItemStatus);
        Items.Add(item);
        return item;
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status != InspectionStatus.InProgress)
        {
            throw new InvalidOperationException($"Cannot remove items from an inspection with status {Status}.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is not null)
        {
            Items.Remove(item);
        }
    }

    public void RemoveItem(InspectionItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        RemoveItem(item.Id);
    }

    // Approve the inspection and emit domain event
    public void Approve()
    {
        if (!HasItems)
            throw new InvalidOperationException("Cannot approve an inspection without items.");

        if (!HasAnyPassed)
            throw new InvalidOperationException("Cannot approve an inspection where no items are passed/accepted.");

        if (Status == InspectionStatus.InProgress)
        {
            ChangeStatus(InspectionStatus.Completed);
        }

        if (Status != InspectionStatus.Completed)
        {
            throw new InvalidOperationException($"Cannot approve an inspection with status {Status}. Must be Completed first.");
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
        if (Status == InspectionStatus.Approved)
        {
            throw new InvalidOperationException("Cannot reject an approved inspection.");
        }

        if (Status == InspectionStatus.Rejected)
        {
            return; // Already rejected
        }

        ChangeStatus(InspectionStatus.Rejected);
        Approved = false;
        
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Remarks = string.IsNullOrWhiteSpace(Remarks) ? reason : $"{Remarks}\nRejection: {reason}";
        }

        QueueDomainEvent(new InspectionRejected
        {
            InspectionId = this.Id,
            PurchaseId = this.PurchaseId,
            EmployeeId = this.EmployeeId,
            RejectedOn = DateTime.UtcNow,
            Reason = reason
        });
    }

    public void Cancel(string? reason = null)
    {
        if (Status == InspectionStatus.Approved)
        {
            throw new InvalidOperationException("Cannot cancel an approved inspection.");
        }

        if (Status == InspectionStatus.Cancelled)
        {
            return; // Already cancelled
        }

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Remarks = string.IsNullOrWhiteSpace(Remarks) ? reason : $"{Remarks}\nCancellation: {reason}";
        }

        ChangeStatus(InspectionStatus.Cancelled);
    }

    public void UpdateRemarks(string? remarks)
    {
        if (Status == InspectionStatus.Approved || Status == InspectionStatus.Cancelled)
        {
            throw new InvalidOperationException($"Cannot update remarks for an inspection with status {Status}.");
        }

        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks;
    }

    public void UpdateIARDocument(string? path)
    {
        IARDocumentPath = string.IsNullOrWhiteSpace(path) ? null : path;
    }

    // Auto-evaluate and set status based on inspection completion
    public void EvaluateAndSetStatus(Purchase? purchase)
    {
        if (Status != InspectionStatus.InProgress)
        {
            return; // Only evaluate for in-progress inspections
        }

        if (purchase is null || purchase.Items.Count == 0)
        {
            return; // If no purchase or no items, stay InProgress
        }

        // Check if all purchase items have been fully inspected
        bool allItemsFullyInspected = true;
        
        foreach (var purchaseItem in purchase.Items)
        {
            // Find corresponding inspection items for this purchase item
            var inspectionItems = Items.Where(ii => ii.PurchaseItemId == purchaseItem.Id).ToList();
            
            if (inspectionItems.Count == 0)
            {
                allItemsFullyInspected = false;
                break;
            }

            // Sum up total inspected quantity for this purchase item
            var totalInspectedQty = inspectionItems.Sum(ii => ii.QtyInspected);
            
            if (totalInspectedQty < purchaseItem.Qty)
            {
                allItemsFullyInspected = false;
                break;
            }
        }

        // Set status based on evaluation
        if (allItemsFullyInspected)
        {
            Complete();
        }
    }

    // Convenience queries
    public IEnumerable<InspectionItem> AcceptedItems() => 
        Items.Where(i => i.InspectionItemStatus == InspectionItemStatus.Passed || 
                        i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation)
             .ToList();

    public IEnumerable<InspectionItem> RejectedItems() => 
        Items.Where(i => i.InspectionItemStatus == InspectionItemStatus.Failed || 
                        i.InspectionItemStatus == InspectionItemStatus.Rejected)
             .ToList();

    // Set inspection date (with validation)
    public void SetInspectedOn(DateTime inspectedOn)
    {
        if (inspectedOn == default)
            throw new ArgumentException("InspectedOn must be a valid date.", nameof(inspectedOn));
        
        if (inspectedOn > DateTime.UtcNow)
            throw new ArgumentException("InspectedOn cannot be in the future.", nameof(inspectedOn));

        InspectedOn = inspectedOn;
    }

    // Update the employee (inspector) if needed
    public void SetEmployee(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("EmployeeId must be provided.", nameof(employeeId));
        
        if (Status == InspectionStatus.Approved)
        {
            throw new InvalidOperationException("Cannot change employee for an approved inspection.");
        }

        if (EmployeeId == employeeId)
            return;

        EmployeeId = employeeId;
        Employee = null!;
    }

    // Update purchase link if needed
    public void SetPurchase(Guid? purchaseId)
    {
        if (Status == InspectionStatus.Approved)
        {
            throw new InvalidOperationException("Cannot change purchase for an approved inspection.");
        }

        PurchaseId = purchaseId;
    }

    public void Complete()
    {
        if (!HasItems)
        {
            throw new InvalidOperationException("Cannot complete an inspection without items.");
        }

        ChangeStatus(InspectionStatus.Completed);
    }

    public void UpdateItem(Guid itemId, int qtyInspected, int qtyPassed, int qtyFailed, string? remarks = null, InspectionItemStatus? inspectionItemStatus = null)
    {
        if (Status != InspectionStatus.InProgress)
        {
            throw new InvalidOperationException($"Cannot update items in an inspection with status {Status}.");
        }

        if (qtyInspected <= 0)
        {
            throw new ArgumentException("Inspected quantity must be greater than zero.", nameof(qtyInspected));
        }

        if (qtyPassed + qtyFailed != qtyInspected)
        {
            throw new ArgumentException("Passed + Failed quantities must equal Inspected quantity.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
        {
            throw new InvalidOperationException($"Inspection item with ID {itemId} not found.");
        }

        item.Update(this.Id, item.PurchaseItemId, qtyInspected, qtyPassed, qtyFailed, remarks, inspectionItemStatus);
    }

    private void ChangeStatus(InspectionStatus newStatus)
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

    private static bool IsValidTransition(InspectionStatus current, InspectionStatus next)
    {
        var validTransitions = new Dictionary<InspectionStatus, InspectionStatus[]>
        {
            { InspectionStatus.InProgress, new[] { InspectionStatus.Completed, InspectionStatus.Rejected, InspectionStatus.Cancelled } },
            { InspectionStatus.Completed, new[] { InspectionStatus.Approved, InspectionStatus.Rejected } },
            { InspectionStatus.Approved, Array.Empty<InspectionStatus>() },
            { InspectionStatus.Rejected, Array.Empty<InspectionStatus>() },
            { InspectionStatus.Cancelled, Array.Empty<InspectionStatus>() }
        };

        return validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}
