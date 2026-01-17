using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Inventories.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain;

public class GoodsReceiptItem : AuditableEntity
{
    public Guid GoodsReceiptId { get; private set; }
    public Guid PurchaseItemId { get; private set; }
    public int QtyReceived { get; private set; }
    public string? Condition { get; private set; }
    public string? Remarks { get; private set; }

    public virtual GoodsReceipt GoodsReceipt { get; private set; } = default!;
    public virtual PurchaseItem PurchaseItem { get; private set; } = default!;

    private GoodsReceiptItem() { }

    private GoodsReceiptItem(Guid id, Guid goodsReceiptId, Guid purchaseItemId, int qtyReceived, string? condition, string? remarks)
    {
        if (goodsReceiptId == Guid.Empty)
            throw new ArgumentException("GoodsReceiptId must be provided.", nameof(goodsReceiptId));
        if (purchaseItemId == Guid.Empty)
            throw new ArgumentException("PurchaseItemId must be provided.", nameof(purchaseItemId));
        if (qtyReceived <= 0)
            throw new ArgumentException("Received quantity must be greater than zero.", nameof(qtyReceived));

        Id = id;
        GoodsReceiptId = goodsReceiptId;
        PurchaseItemId = purchaseItemId;
        QtyReceived = qtyReceived;
        Condition = condition;
        Remarks = remarks;

        QueueDomainEvent(new GoodsReceiptItemAdded { GoodsReceiptItem = this });
    }

    public static GoodsReceiptItem Create(Guid goodsReceiptId, Guid purchaseItemId, int qtyReceived, string? condition, string? remarks)
        => new(Guid.NewGuid(), goodsReceiptId, purchaseItemId, qtyReceived, condition, remarks);

    public GoodsReceiptItem Update(int qtyReceived, string? condition, string? remarks)
    {
        if (qtyReceived <= 0)
            throw new ArgumentException("Received quantity must be greater than zero.", nameof(qtyReceived));

        bool updated = false;

        if (QtyReceived != qtyReceived)
        {
            QtyReceived = qtyReceived;
            updated = true;
        }

        if (Condition != condition)
        {
            Condition = condition;
            updated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            updated = true;
        }

        if (updated)
        {
            QueueDomainEvent(new GoodsReceiptUpdated { GoodsReceiptId = GoodsReceiptId });
        }

        return this;
    }
}

