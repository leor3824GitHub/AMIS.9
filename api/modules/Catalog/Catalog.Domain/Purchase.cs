using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public Guid SupplierId { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Status { get; private set; }
    public virtual Supplier Supplier { get; private set; } = default!;

    private Purchase() { }

    private Purchase(Guid id, Guid supplierId, DateTime purchaseDate, decimal totalAmount, string? status)
    {
        Id = id;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate;
        TotalAmount = totalAmount;
        Status = status;

        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public static Purchase Create(Guid supplierId, DateTime purchaseDate, decimal totalAmount, string? status)
    {
        return new Purchase(Guid.NewGuid(), supplierId, purchaseDate, totalAmount, status);
    }

    public Purchase Update(Guid supplierId, DateTime purchaseDate, decimal totalAmount, string? status)
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

        return this;
    }
}

