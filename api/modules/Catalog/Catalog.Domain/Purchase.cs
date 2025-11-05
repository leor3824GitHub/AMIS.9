using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public Guid? SupplierId { get; private set; }
    public DateTime? PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; } = 0;
    public PurchaseStatus? Status { get; private set; }
    public virtual Supplier? Supplier { get; private set; }
    public virtual ICollection<PurchaseItem> Items { get; private set; } = [];

    public virtual ICollection<Inspection> Inspections { get; private set; } = [];
    public virtual ICollection<Acceptance> Acceptances { get; private set; } = [];

    // Add summary helpers (not mapped)
    public bool IsFullyInspected => Items.All(i => i.InspectionStatus == PurchaseItemInspectionStatus.Passed);
    public bool IsFullyAccepted => Items.All(i => i.AcceptanceStatus == PurchaseItemAcceptanceStatus.Accepted);
    private Purchase() { }

    private Purchase(Guid id, Guid? supplierId, DateTime? purchaseDate, decimal totalAmount, PurchaseStatus? status)
    {
        Id = id;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate;
        TotalAmount = totalAmount;
        Status = status;

        QueueDomainEvent(new PurchaseCreated { Purchase = this });
    }

    public void AddItem(Guid? productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        var item = PurchaseItem.Create(this.Id, productId, qty, unitPrice, status);
        Items.Add(item);
    }

    public void AddItem(Guid id, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        var item = PurchaseItem.Create(id, productId, qty, unitPrice, status);
        Items.Add(item);
    }

    public static Purchase Create(Guid? supplierId, DateTime? purchaseDate, decimal totalAmount, PurchaseStatus? status)
    {
        return new Purchase(Guid.NewGuid(), supplierId, purchaseDate, totalAmount, status);
    }

    public Purchase Update(Guid? supplierId, DateTime? purchaseDate, decimal totalAmount, PurchaseStatus? status)
    {
        bool isUpdated = false;

        if (SupplierId != supplierId)
        {
            SupplierId = supplierId;
            isUpdated = true;
        }

        if (PurchaseDate != purchaseDate)
        {
            PurchaseDate = purchaseDate;
            isUpdated = true;
        }

        if (TotalAmount != totalAmount)
        {
            TotalAmount = totalAmount;
            isUpdated = true;
        }

        if (!Nullable.Equals(Status, status))
        {
            Status = status;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseUpdated { Purchase = this });
        }

        return this;
    }

    // Purchase Order Workflow Methods
    public void SubmitForApproval()
    {
        if (Status != PurchaseStatus.Draft)
            throw new InvalidOperationException("Only draft purchases can be submitted for approval.");
        
        Status = PurchaseStatus.PendingApproval;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void Approve()
    {
        if (Status != PurchaseStatus.PendingApproval)
            throw new InvalidOperationException("Only pending purchases can be approved.");
        
        Status = PurchaseStatus.Approved;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void Reject(string reason)
    {
        if (Status != PurchaseStatus.PendingApproval)
            throw new InvalidOperationException("Only pending purchases can be rejected.");
        
        Status = PurchaseStatus.Rejected;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void Acknowledge()
    {
        if (Status != PurchaseStatus.Approved)
            throw new InvalidOperationException("Only approved purchases can be acknowledged.");
        
        Status = PurchaseStatus.Acknowledged;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkInProgress()
    {
        if (Status != PurchaseStatus.Acknowledged)
            throw new InvalidOperationException("Only acknowledged purchases can be marked in progress.");
        
        Status = PurchaseStatus.InProgress;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkShipped()
    {
        if (Status != PurchaseStatus.InProgress)
            throw new InvalidOperationException("Only in-progress purchases can be marked as shipped.");
        
        Status = PurchaseStatus.Shipped;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkPartiallyReceived()
    {
        if (Status != PurchaseStatus.Shipped && Status != PurchaseStatus.Approved)
            throw new InvalidOperationException("Purchase must be shipped or approved to mark as partially received.");
        
        Status = PurchaseStatus.PartiallyReceived;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkFullyReceived()
    {
        if (!Items.All(i => i.AcceptanceStatus == PurchaseItemAcceptanceStatus.Accepted || 
                           i.AcceptanceStatus == PurchaseItemAcceptanceStatus.AcceptedWithDeviation))
            throw new InvalidOperationException("All items must be accepted before marking as fully received.");
        
        Status = PurchaseStatus.FullyReceived;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkPendingInvoice()
    {
        if (Status != PurchaseStatus.FullyReceived && Status != PurchaseStatus.PartiallyReceived)
            throw new InvalidOperationException("Purchase must be received before marking as pending invoice.");
        
        Status = PurchaseStatus.PendingInvoice;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkInvoiced()
    {
        if (Status != PurchaseStatus.PendingInvoice)
            throw new InvalidOperationException("Purchase must be pending invoice before marking as invoiced.");
        
        Status = PurchaseStatus.Invoiced;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkPendingPayment()
    {
        if (Status != PurchaseStatus.Invoiced)
            throw new InvalidOperationException("Purchase must be invoiced before marking as pending payment.");
        
        Status = PurchaseStatus.PendingPayment;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void MarkClosed()
    {
        if (Status != PurchaseStatus.PendingPayment && Status != PurchaseStatus.Invoiced)
            throw new InvalidOperationException("Purchase must be invoiced or pending payment before closing.");
        
        Status = PurchaseStatus.Closed;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void PutOnHold(string reason)
    {
        if (Status == PurchaseStatus.Closed || Status == PurchaseStatus.Cancelled)
            throw new InvalidOperationException("Cannot put closed or cancelled purchases on hold.");
        
        Status = PurchaseStatus.OnHold;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void ReleaseFromHold()
    {
        if (Status != PurchaseStatus.OnHold)
            throw new InvalidOperationException("Only purchases on hold can be released.");
        
        Status = PurchaseStatus.Draft; // Return to draft for re-evaluation
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void Cancel(string reason)
    {
        if (Status == PurchaseStatus.Closed)
            throw new InvalidOperationException("Cannot cancel closed purchases.");
        
        Status = PurchaseStatus.Cancelled;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }
    
}


