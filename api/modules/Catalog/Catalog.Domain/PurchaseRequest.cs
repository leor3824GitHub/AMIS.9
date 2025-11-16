using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class PurchaseRequest : AuditableEntity, IAggregateRoot
{
    public DateTime RequestDate { get; private set; }
    // FK to Employee
    public Guid RequestedBy { get; private set; }
    // Navigation to Employee who requested
    public virtual Employee? RequestedByEmployee { get; private set; }
    public string Purpose { get; private set; } = string.Empty;
    public PurchaseRequestStatus Status { get; private set; }
    public string? ApprovalRemarks { get; private set; }
    public Guid? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    
    public virtual ICollection<PurchaseRequestItem> Items { get; private set; } = [];

    // Computed properties - not persisted
    public bool HasItems => Items.Count > 0;
    public int TotalItemsCount => Items.Sum(i => i.Qty);

    private PurchaseRequest() { }

    private PurchaseRequest(Guid id, DateTime requestDate, Guid requestedBy, string purpose, PurchaseRequestStatus status)
    {
        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("Purpose is required.", nameof(purpose));

        Id = id;
        RequestDate = requestDate;
        RequestedBy = requestedBy;
        Purpose = purpose;
        Status = status;

        QueueDomainEvent(new PurchaseRequestCreated { PurchaseRequest = this });
    }

    public static PurchaseRequest Create(Guid requestedBy, string purpose, DateTime? requestDate = null)
    {
        var purchaseRequest = new PurchaseRequest(
            Guid.NewGuid(), 
            requestDate ?? DateTime.UtcNow, 
            requestedBy, 
            purpose, 
            PurchaseRequestStatus.Draft);
        
        return purchaseRequest;
    }

    public PurchaseRequest Update(string purpose)
    {
        if (Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot modify a {Status} purchase request.");
        }

        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("Purpose is required.", nameof(purpose));

        bool isUpdated = false;

        if (Purpose != purpose)
        {
            Purpose = purpose;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseRequestUpdated { PurchaseRequest = this });
        }

        return this;
    }

    public void AddItem(Guid? productId, int qty, string? description, string? justification)
    {
        if (Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot add items to a {Status} purchase request.");
        }

        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        }

        var item = PurchaseRequestItem.Create(this.Id, productId, qty, description, justification);
        Items.Add(item);
    }

    public void AddItem(Guid id, Guid? productId, int qty, string? description, string? justification)
    {
        if (Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot add items to a {Status} purchase request.");
        }

        var item = PurchaseRequestItem.Create(id, this.Id, productId, qty, description, justification);
        Items.Add(item);
    }

    public void UpdateItem(Guid itemId, Guid? productId, int qty, string? description, string? justification)
    {
        if (Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot update items in a {Status} purchase request.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
        {
            throw new InvalidOperationException($"Purchase request item with ID {itemId} not found.");
        }

        item.Update(productId, qty, description, justification);
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot remove items from a {Status} purchase request.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is not null)
        {
            Items.Remove(item);
        }
    }

    public void Submit()
    {
        if (Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot submit a purchase request with status {Status}.");
        }

        if (Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot submit a purchase request without items.");
        }

        Status = PurchaseRequestStatus.Submitted;
        QueueDomainEvent(new PurchaseRequestSubmitted { PurchaseRequestId = Id });
        QueueDomainEvent(new PurchaseRequestUpdated { PurchaseRequest = this });
    }

    public void Approve(Guid approvedBy, string? remarks = null)
    {
        if (Status != PurchaseRequestStatus.Submitted)
        {
            throw new InvalidOperationException($"Cannot approve a purchase request with status {Status}. Must be Submitted first.");
        }

        Status = PurchaseRequestStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalRemarks = remarks;

        QueueDomainEvent(new PurchaseRequestApproved { PurchaseRequestId = Id });
        QueueDomainEvent(new PurchaseRequestUpdated { PurchaseRequest = this });
    }

    public void Reject(Guid rejectedBy, string reason)
    {
        if (Status != PurchaseRequestStatus.Submitted)
        {
            throw new InvalidOperationException($"Cannot reject a purchase request with status {Status}. Must be Submitted first.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Rejection reason is required.", nameof(reason));
        }

        Status = PurchaseRequestStatus.Rejected;
        ApprovedBy = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalRemarks = reason;

        QueueDomainEvent(new PurchaseRequestRejected { PurchaseRequestId = Id, Reason = reason });
        QueueDomainEvent(new PurchaseRequestUpdated { PurchaseRequest = this });
    }

    public void Cancel(string? reason = null)
    {
        if (Status == PurchaseRequestStatus.Approved)
        {
            throw new InvalidOperationException("Cannot cancel an approved purchase request.");
        }

        if (Status == PurchaseRequestStatus.Cancelled)
        {
            return; // Already cancelled
        }

        Status = PurchaseRequestStatus.Cancelled;
        ApprovalRemarks = string.IsNullOrWhiteSpace(reason) ? ApprovalRemarks : $"{ApprovalRemarks}\nCancellation Reason: {reason}";
        
        QueueDomainEvent(new PurchaseRequestUpdated { PurchaseRequest = this });
    }
}
