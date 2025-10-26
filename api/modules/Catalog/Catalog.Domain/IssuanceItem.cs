using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class IssuanceItem : AuditableEntity, IAggregateRoot
{
    public Guid IssuanceId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Status { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private IssuanceItem() { }

    private IssuanceItem(Guid id, Guid issuanceId, Guid productId, int qty, decimal unitPrice, string? status)
    {
        Id = id;
        IssuanceId = issuanceId;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        Status = status;

        QueueDomainEvent(new IssuanceItemCreated { IssuanceItem = this });
    }

    public static IssuanceItem Create(Guid issuanceId, Guid productId, int qty, decimal unitPrice, string? status)
    {
        Validate(qty, unitPrice);
        return new IssuanceItem(Guid.NewGuid(), issuanceId, productId, qty, unitPrice, status);
    }

    public IssuanceItem Update(Guid issuanceId, Guid productId, int qty, decimal unitPrice, string? status)
    {
        Validate(qty, unitPrice);
        bool isUpdated = false;

        if (IssuanceId != issuanceId)
        {
            IssuanceId = issuanceId;
            isUpdated = true;
        }

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (Qty != qty)
        {
            Qty = qty;
            isUpdated = true;
        }

        if (UnitPrice != unitPrice)
        {
            UnitPrice = unitPrice;
            isUpdated = true;
        }

        if (!string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
        {
            Status = status;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new IssuanceItemUpdated { IssuanceItem = this });
        }

        return this;
    }

    private static void Validate(int qty, decimal unitPrice)
    {
        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price must not be negative.", nameof(unitPrice));
        }
    }
}

