using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class Acceptance : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid SupplyOfficerId { get; private set; } // Accountable Employee ID
    public Guid? InspectionId { get; private set; }
    public DateTime AcceptanceDate { get; private set; }
    public string? Remarks { get; private set; }
    public bool IsPosted { get; private set; }
    public DateTime? PostedOn { get; private set; }
    public AcceptanceStatus Status { get; private set; }

    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Employee SupplyOfficer { get; private set; } = default!;
    public virtual Inspection? Inspection { get; private set; }
    public virtual ICollection<AcceptanceItem> Items { get; private set; } = [];

    // Computed properties
    public bool HasItems => Items.Any();
    public int TotalAcceptedQuantity => Items.Sum(i => i.QtyAccepted);
    public bool IsFullAcceptance => Items.Any() && Items.All(i => i.QtyAccepted >= i.PurchaseItem.Qty);
    public bool IsPartialAcceptance => Items.Any(i => i.QtyAccepted < i.PurchaseItem.Qty && i.QtyAccepted > 0);

    private Acceptance() { }

    private Acceptance(Guid id, Guid purchaseId, Guid supplyOfficerId, Guid? inspectionId, DateTime acceptanceDate, string? remarks)
    {
        Id = id;
        PurchaseId = purchaseId;
        SupplyOfficerId = supplyOfficerId;
        InspectionId = inspectionId;
        AcceptanceDate = acceptanceDate;
        Remarks = remarks;
        Status = AcceptanceStatus.Pending;
        IsPosted = false;

        QueueDomainEvent(new AcceptanceCreated { Acceptance = this });
    }

    public static Acceptance Create(Guid purchaseId, Guid supplyOfficerId, DateTime acceptanceDate, string? remarks, Guid? inspectionId = null)
    {
        if (purchaseId == Guid.Empty)
            throw new ArgumentException("PurchaseId must be provided.", nameof(purchaseId));
        
        if (supplyOfficerId == Guid.Empty)
            throw new ArgumentException("SupplyOfficerId must be provided.", nameof(supplyOfficerId));

        if (acceptanceDate == default)
            throw new ArgumentException("AcceptanceDate must be provided.", nameof(acceptanceDate));

        if (acceptanceDate > DateTime.UtcNow)
            throw new ArgumentException("AcceptanceDate cannot be in the future.", nameof(acceptanceDate));

        return new Acceptance(Guid.NewGuid(), purchaseId, supplyOfficerId, inspectionId, acceptanceDate, remarks);
    }

    public Acceptance Update(Guid supplyOfficerId, DateTime acceptanceDate, string? remarks)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot modify a posted acceptance.");
        }

        if (Status == AcceptanceStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot modify a cancelled acceptance.");
        }

        bool isUpdated = false;

        if (SupplyOfficerId != supplyOfficerId)
        {
            if (supplyOfficerId == Guid.Empty)
                throw new ArgumentException("SupplyOfficerId must be provided.", nameof(supplyOfficerId));
            
            SupplyOfficerId = supplyOfficerId;
            isUpdated = true;
        }

        if (AcceptanceDate != acceptanceDate)
        {
            if (acceptanceDate == default)
                throw new ArgumentException("AcceptanceDate must be provided.", nameof(acceptanceDate));
            
            if (acceptanceDate > DateTime.UtcNow)
                throw new ArgumentException("AcceptanceDate cannot be in the future.", nameof(acceptanceDate));
            
            AcceptanceDate = acceptanceDate;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new AcceptanceUpdated { Acceptance = this });
        }

        return this;
    }

    public void AddItem(Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot add items to a posted acceptance.");
        }

        if (Status == AcceptanceStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot add items to a cancelled acceptance.");
        }

        if (qtyAccepted <= 0)
        {
            throw new ArgumentException("Accepted quantity must be greater than zero.", nameof(qtyAccepted));
        }

        // Check if item already exists for this purchase item
        if (Items.Any(i => i.PurchaseItemId == purchaseItemId))
        {
            throw new InvalidOperationException($"Acceptance item for purchase item {purchaseItemId} already exists.");
        }

        var item = AcceptanceItem.Create(Id, purchaseItemId, qtyAccepted, remarks);
        Items.Add(item);

        QueueDomainEvent(new AcceptanceUpdated { Acceptance = this });
    }

    public void UpdateItem(Guid itemId, int qtyAccepted, string? remarks)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot update items in a posted acceptance.");
        }

        if (Status == AcceptanceStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot update items in a cancelled acceptance.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
        {
            throw new InvalidOperationException($"Acceptance item with ID {itemId} not found.");
        }

        item.Update(qtyAccepted, remarks);
        QueueDomainEvent(new AcceptanceUpdated { Acceptance = this });
    }

    public void RemoveItem(Guid itemId)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot remove items from a posted acceptance.");
        }

        if (Status == AcceptanceStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot remove items from a cancelled acceptance.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is not null)
        {
            Items.Remove(item);
            QueueDomainEvent(new AcceptanceUpdated { Acceptance = this });
        }
    }

    public void LinkInspection(Guid inspectionId)
    {
        if (inspectionId == Guid.Empty)
        {
            throw new ArgumentException("InspectionId must be provided.", nameof(inspectionId));
        }

        if (InspectionId.HasValue && InspectionId.Value != inspectionId)
        {
            throw new InvalidOperationException("Acceptance is already linked to a different inspection.");
        }

        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot link inspection to a posted acceptance.");
        }

        InspectionId = inspectionId;
    }

    public void PostAcceptance(DateTime? postedOnUtc = null)
    {
        if (IsPosted)
        {
            return; // Already posted
        }

        if (!HasItems)
        {
            throw new InvalidOperationException("Cannot post an acceptance without any items.");
        }

        if (Status == AcceptanceStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot post a cancelled acceptance.");
        }

        IsPosted = true;
        PostedOn = postedOnUtc ?? DateTime.UtcNow;
        Status = AcceptanceStatus.Posted;

        QueueDomainEvent(new AcceptancePosted { AcceptanceId = Id });
    }

    public void Cancel(string? reason = null)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot cancel a posted acceptance.");
        }

        if (Status == AcceptanceStatus.Cancelled)
        {
            return; // Already cancelled
        }

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Remarks = string.IsNullOrWhiteSpace(Remarks) ? reason : $"{Remarks}\nCancellation: {reason}";
        }

        Status = AcceptanceStatus.Cancelled;
        QueueDomainEvent(new AcceptanceUpdated { Acceptance = this });
    }

    public void ValidateAgainstInspection(Inspection inspection)
    {
        if (inspection.PurchaseId != this.PurchaseId)
        {
            throw new InvalidOperationException("Acceptance and Inspection must be for the same Purchase.");
        }

        if (inspection.Status != InspectionStatus.Approved)
        {
            throw new InvalidOperationException("Cannot accept items from a non-approved inspection.");
        }

        foreach (var acceptanceItem in Items)
        {
            var inspectionItem = inspection.Items.FirstOrDefault(i => i.PurchaseItemId == acceptanceItem.PurchaseItemId);
            
            if (inspectionItem is null)
            {
                throw new InvalidOperationException($"Purchase item {acceptanceItem.PurchaseItemId} was not inspected.");
            }

            if (acceptanceItem.QtyAccepted > inspectionItem.QtyPassed)
            {
                throw new InvalidOperationException($"Cannot accept more quantity ({acceptanceItem.QtyAccepted}) than passed inspection ({inspectionItem.QtyPassed}) for purchase item {acceptanceItem.PurchaseItemId}.");
            }
        }
    }
}
