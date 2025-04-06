using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public Guid? SupplierId { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; }
    public virtual Supplier Supplier { get; private set; } = default!;

    private Purchase() { }

    private Purchase(Guid id, Guid? supplierId, DateTime purchaseDate, decimal totalAmount)
    {
        Id = id;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate;
        TotalAmount = totalAmount;
        Status = "InProgress";

        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public static Purchase Create(Guid? supplierId, DateTime purchaseDate, decimal totalAmount)
    {
        return new Purchase(Guid.NewGuid(), supplierId, purchaseDate, totalAmount);
    }

    public Purchase Update(Guid? supplierId, DateTime purchaseDate, decimal totalAmount, string? status)
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

        if (!string.IsNullOrEmpty(status) && Status != status) // Safely handle nullable string
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

