using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class PurchaseItem : BaseEntity, ISoftDeletable
{
    public Guid PurchaseId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public PurchaseStatus? ItemStatus { get; private set; }

    public DateTimeOffset? Deleted { get; set; }
    public Guid? DeletedBy { get; set; }

    private PurchaseItem() { }

    private PurchaseItem(Guid id, Guid purchaseId, Guid productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        Id = id;
        PurchaseId = purchaseId;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        ItemStatus = status;
    }

    public static PurchaseItem Create(Guid purchaseId, Guid productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        return new PurchaseItem(Guid.NewGuid(), purchaseId, productId, qty, unitPrice, status);
    }

    public static PurchaseItem CreateWithId(Guid id, Guid purchaseId, Guid productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        return new PurchaseItem(id, purchaseId, productId, qty, unitPrice, status);
    }
    public PurchaseItem Update(Guid productId, int qty, decimal unitPrice, PurchaseStatus? itemstatus)
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

        if (!Nullable.Equals(ItemStatus, itemstatus))
        {
            ItemStatus = itemstatus;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = this });
        }

        return this;
    }
}

