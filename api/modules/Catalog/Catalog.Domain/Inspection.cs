using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents a purchase item summary for cross-aggregate status evaluation.
/// This record is used to pass data across aggregate boundaries without coupling to Purchase entity.
/// </summary>
public record PurchaseItemSummary(Guid Id, int Qty);

public class Inspection : AuditableEntity, IAggregateRoot
{
    // Link to InspectionRequest (optional) - ID only to maintain aggregate boundaries
    public Guid? InspectionRequestId { get; private set; }
    
    /// <summary>
    /// Navigation property for EF Core materialization and query projections ONLY.
    /// <para>
    /// ⚠️ WARNING: Do NOT use this property in business logic or command handlers.
    /// Use InspectionRequestId and fetch data from repositories instead to maintain proper aggregate boundaries.
    /// </para>
    /// <para>
    /// This property is public only to allow EF Core specifications in the Application layer to project DTOs.
    /// Following DDD principles, aggregates should reference each other by ID, not by navigation.
    /// </para>
    /// </summary>
    [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Navigation property required by EF Core for projections")]
    public InspectionRequest? InspectionRequest { get; private set; }

    // Employee who performed the inspection
    public Guid EmployeeId { get; private set; }
    public virtual Employee Employee { get; private set; } = default!;

    // Inspection details
    public DateTime? InspectedOn { get; private set; }
    public bool Approved { get; private set; }
    public InspectionStatus Status { get; private set; }
    public string? Remarks { get; private set; }
    public string? IARDocumentPath { get; private set; }

    // Use InspectionItem for line items
    public virtual ICollection<InspectionItem> Items { get; private set; } = new List<InspectionItem>();

    // Parameterless ctor for EF
    private Inspection() { }

    // Internal constructor - use factory Create(...) for creation
    private Inspection(Guid id, Guid? inspectionRequestId, Guid employeeId, DateTime inspectedOn, bool approved, string? remarks, string? iarDocumentPath)
    {
        Id = id;
        InspectionRequestId = inspectionRequestId;
        EmployeeId = employeeId;
        InspectedOn = inspectedOn;
        Approved = approved;
        Remarks = remarks;
        IARDocumentPath = iarDocumentPath;
        Items = new List<InspectionItem>();
        Status = InspectionStatus.InProgress;
    }

    // Factory - ensures Id is generated and sensible defaults are applied
    public static Inspection Create(Guid? inspectionRequestId, Guid employeeId, DateTime? inspectedOn = null, bool approved = false, string? remarks = null, string? iarDocumentPath = null)
    {
        if (employeeId == Guid.Empty) throw new ArgumentException("EmployeeId must be provided.", nameof(employeeId));
        
        var inspectedAt = inspectedOn ?? DateTime.UtcNow;
        var inspection = new Inspection(Guid.NewGuid(), inspectionRequestId, employeeId, inspectedAt, approved, remarks, iarDocumentPath);

        // Queue domain event for creation
        inspection.QueueDomainEvent(new InspectionCreated
        {
            InspectionId = inspection.Id,
            InspectionRequestId = inspectionRequestId,
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
            // detach navigation - keep InspectionId as-is for audit/history
        }
    }

    // Approve the inspection and emit domain event
    /// <summary>
    /// Approves the inspection and queues the InspectionApproved domain event.
    /// </summary>
    /// <param name="purchaseId">Optional PurchaseId to include in the event. If not provided, 
    /// handlers should look it up from InspectionRequest.</param>
    public void Approve(Guid? purchaseId = null)
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
            InspectionRequestId = this.InspectionRequestId,
            EmployeeId = this.EmployeeId,
            ApprovedOn = DateTime.UtcNow,
            PurchaseId = purchaseId ?? Guid.Empty
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
            InspectionRequestId = this.InspectionRequestId,
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

    /// <summary>
    /// Auto-evaluate and set status based on inspection completion.
    /// Accepts purchase item data (not entities) to avoid cross-aggregate dependencies.
    /// </summary>
    /// <param name="purchaseItems">Summary of purchase items with ID and quantity</param>
    public void EvaluateAndSetStatus(IReadOnlyCollection<PurchaseItemSummary>? purchaseItems)
    {
        if (purchaseItems is null || purchaseItems.Count == 0)
        {
            // If no purchase items data provided, stay InProgress
            return;
        }

        // Check if all purchase items have been fully inspected
        bool allItemsFullyInspected = true;
        
        foreach (var purchaseItem in purchaseItems)
        {
            // Find corresponding inspection items for this purchase item
            var inspectionItems = Items.Where(ii => ii.PurchaseItemId == purchaseItem.Id).ToList();
  
            if (inspectionItems.Count == 0)
            {
                // This purchase item hasn't been inspected at all
                allItemsFullyInspected = false;
                break;
            }

            // Sum up total inspected quantity for this purchase item
            var totalInspectedQty = inspectionItems.Sum(ii => ii.QtyInspected);
      
            if (totalInspectedQty < purchaseItem.Qty)
            {
                // This purchase item hasn't been fully inspected
                allItemsFullyInspected = false;
                break;
            }
        }

        // Set status based on evaluation
        if (allItemsFullyInspected && Status == InspectionStatus.InProgress)
        {
            ChangeStatus(InspectionStatus.Completed);
        }
        // If not all items inspected, it stays InProgress (partial inspection)
    }

    // Convenience queries
    public IEnumerable<InspectionItem> AcceptedItems() => 
        Items.Where(i => i.InspectionItemStatus == InspectionItemStatus.Passed || 
            i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation).ToList();

    public int TotalInspectedQuantity() => Items.Sum(i => i.QtyInspected);

    public int TotalAcceptedQuantity() => 
        Items.Where(i => i.InspectionItemStatus == InspectionItemStatus.Passed || 
            i.InspectionItemStatus == InspectionItemStatus.AcceptedWithDeviation)
            .Sum(i => i.QtyPassed);

    // Set inspection date (with validation)
    public void SetInspectedOn(DateTime inspectedOn)
    {
        if (inspectedOn == default)
        {
            throw new ArgumentException("InspectedOn must be a valid date.", nameof(inspectedOn));
        }
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

    // Update inspection request link if needed
    public void SetInspectionRequest(Guid? inspectionRequestId)
    {
        if (InspectionRequestId == inspectionRequestId) return;
        InspectionRequestId = inspectionRequestId;
        InspectionRequest = null!;
    }

    public void ChangeStatus(InspectionStatus newStatus)
    {
        if (Status == newStatus)
        {
            return;
        }

        if (!IsValidTransition(Status, newStatus))
        {
            throw new InvalidInspectionTransitionException(Id, Status, newStatus);
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
            { InspectionStatus.Scheduled, new[] { InspectionStatus.InProgress, InspectionStatus.Cancelled, InspectionStatus.OnHold } },
            { InspectionStatus.InProgress, new[] { InspectionStatus.Completed, InspectionStatus.Approved, InspectionStatus.Rejected, InspectionStatus.Cancelled, InspectionStatus.OnHold, InspectionStatus.Quarantined } },
            { InspectionStatus.Completed, new[] { InspectionStatus.Approved, InspectionStatus.Rejected, InspectionStatus.ConditionallyApproved, InspectionStatus.PartiallyApproved } },
            { InspectionStatus.Approved, new[] { InspectionStatus.ReInspectionRequired } },
            { InspectionStatus.ConditionallyApproved, new[] { InspectionStatus.Approved, InspectionStatus.Rejected } },
            { InspectionStatus.PartiallyApproved, new[] { InspectionStatus.Approved, InspectionStatus.Rejected } },
            { InspectionStatus.Rejected, new[] { InspectionStatus.ReInspectionRequired } },
            { InspectionStatus.OnHold, new[] { InspectionStatus.InProgress, InspectionStatus.Cancelled } },
            { InspectionStatus.Quarantined, new[] { InspectionStatus.InProgress, InspectionStatus.Approved, InspectionStatus.Rejected } },
            { InspectionStatus.ReInspectionRequired, new[] { InspectionStatus.Scheduled, InspectionStatus.InProgress } },
            { InspectionStatus.Cancelled, Array.Empty<InspectionStatus>() }
        };

        return validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }

    // Schedule inspection with date
    public void Schedule(DateTime scheduledDate)
    {
        if (Status != InspectionStatus.Scheduled)
            throw new InvalidInspectionStatusException(Id, Status.ToString(), "schedule");
        
        if (scheduledDate < DateTime.UtcNow)
            throw InspectionValidationException.ForInvalidScheduledDate();

        SetInspectedOn(scheduledDate);
    }

    // Conditionally approve with deviations
    public void ConditionallyApprove(string conditions)
    {
        if (string.IsNullOrWhiteSpace(conditions))
            throw InspectionValidationException.ForMissingConditions();

        ChangeStatus(InspectionStatus.ConditionallyApproved);
        UpdateRemarks($"Conditionally Approved: {conditions}");
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    // Partially approve inspection
    public void PartiallyApprove(string partialDetails)
    {
        if (string.IsNullOrWhiteSpace(partialDetails))
            throw InspectionValidationException.ForMissingPartialDetails();

        ChangeStatus(InspectionStatus.PartiallyApproved);
        UpdateRemarks($"Partially Approved: {partialDetails}");
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    // Put inspection on hold
    public void PutOnHold(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw InspectionValidationException.ForMissingHoldReason();

        ChangeStatus(InspectionStatus.OnHold);
        UpdateRemarks($"On Hold: {reason}");
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    // Release from hold
    public void ReleaseFromHold()
    {
        if (Status != InspectionStatus.OnHold)
            throw new InvalidInspectionStatusException(Id, Status.ToString(), "release from hold");

        ChangeStatus(InspectionStatus.InProgress);
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    // Quarantine inspection
    public void Quarantine(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw InspectionValidationException.ForMissingQuarantineReason();

        ChangeStatus(InspectionStatus.Quarantined);
        UpdateRemarks($"Quarantined: {reason}");
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    // Release from quarantine
    public void ReleaseFromQuarantine()
    {
        if (Status != InspectionStatus.Quarantined)
            throw new InvalidInspectionStatusException(Id, Status.ToString(), "release from quarantine");

        ChangeStatus(InspectionStatus.InProgress);
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }

    // Require re-inspection
    public void RequireReInspection(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw InspectionValidationException.ForMissingReInspectionReason();

        ChangeStatus(InspectionStatus.ReInspectionRequired);
        UpdateRemarks($"Re-Inspection Required: {reason}");
        QueueDomainEvent(new InspectionUpdated { Inspection = this });
    }
}
