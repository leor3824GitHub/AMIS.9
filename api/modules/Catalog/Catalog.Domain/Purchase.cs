using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public Guid? SupplierId { get; private set; }
    public DateTime? PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; } 
    public PurchaseStatus? Status { get; private set; }

    public virtual Supplier? Supplier { get; private set; }
    public virtual ICollection<PurchaseItem> Items { get; private set; } = [];

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

    public static Purchase Create(Guid? supplierId, DateTime? purchaseDate, decimal totalAmount, PurchaseStatus? status)
    {
        return new Purchase(Guid.NewGuid(), supplierId, purchaseDate, totalAmount, status);
    }

    public void Update(Guid? supplierId, DateTime? purchaseDate, decimal totalAmount, PurchaseStatus? status)
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

        if (Status != status)
        {
            Status = status;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseUpdated { Purchase = this });
        }
    }

    public void AddItem(PurchaseItem item)
    {
        Items.Add(item);
        QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = item });
    }

    public void RemoveItem(PurchaseItem item)
    {
        Items.Remove(item); // EF handles soft delete via interceptor
        QueueDomainEvent(new PurchaseItemRemoved { PurchaseItem = item });
    }
}

