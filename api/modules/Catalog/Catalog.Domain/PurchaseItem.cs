﻿using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class PurchaseItem : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Status { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private PurchaseItem() { }

    private PurchaseItem(Guid id, Guid purchaseId, Guid productId, int qty, decimal unitPrice, string? status)
    {
        Id = id;
        PurchaseId = purchaseId;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        Status = status;

        QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = this });
    }

    public static PurchaseItem Create(Guid purchaseId, Guid productId, int qty, decimal unitPrice, string? status)
    {
        return new PurchaseItem(Guid.NewGuid(), purchaseId, productId, qty, unitPrice, status);
    }

    public PurchaseItem Update(Guid productId, int qty, decimal unitPrice, string? status)
    {
        bool isUpdated = false;             

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
            QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = this });
        }

        return this;
    }
}

