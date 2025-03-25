using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class IssuanceItem : AuditableEntity, IAggregateRoot
{
    public Guid IssuanceId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal Qty { get; private set; }
    public string Unit { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public string? Status { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private IssuanceItem() { }

    private IssuanceItem(Guid id, Guid issuanceId, Guid productId, decimal qty, string unit, decimal unitPrice, string? status)
    {
        Id = id;
        IssuanceId = issuanceId;
        ProductId = productId;
        Qty = qty;
        Unit = unit;
        UnitPrice = unitPrice;
        Status = status;

        QueueDomainEvent(new IssuanceItemUpdated { IssuanceItem = this });
    }

    public static IssuanceItem Create(Guid issuanceId, Guid productId, decimal qty, string unit, decimal unitPrice, string? status)
    {
        return new IssuanceItem(Guid.NewGuid(), issuanceId, productId, qty, unit, unitPrice, status);
    }

    public IssuanceItem Update(Guid issuanceId, Guid productId, decimal qty, string unit, decimal unitPrice, string? status)
    {
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

        if (!string.Equals(Unit, unit, StringComparison.OrdinalIgnoreCase))
        {
            Unit = unit;
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
}

