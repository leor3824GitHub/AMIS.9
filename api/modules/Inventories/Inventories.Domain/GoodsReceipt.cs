using System.Collections.Generic;
using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Records deliveries against a Purchase Order (Purchase aggregate).
/// Enables partial receipts and provides the hand-off to inspection.
/// </summary>
public class GoodsReceipt : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid? SupplierId { get; private set; }
    public Guid ReceivedById { get; private set; }
    public DateTime ReceivedOn { get; private set; }
    public string? DeliveryNoteNumber { get; private set; }
    public string? Remarks { get; private set; }
    public GoodsReceiptStatus Status { get; private set; }

    public virtual Purchase Purchase { get; init; } = default!;
    public virtual Employee ReceivedBy { get; init; } = default!;
    public virtual Supplier? Supplier { get; init; }
    public virtual ICollection<GoodsReceiptItem> Items { get; private set; } = new List<GoodsReceiptItem>();

    // Computed helpers
    public bool IsPosted => Status == GoodsReceiptStatus.Posted;
    public int TotalLines => Items.Count;
    public int TotalQuantity => Items.Sum(i => i.QtyReceived);

    private GoodsReceipt() { }

    private GoodsReceipt(
        Guid id,
        Guid purchaseId,
        Guid? supplierId,
        Guid receivedById,
        DateTime receivedOn,
        string? deliveryNoteNumber,
        string? remarks)
    {
        if (purchaseId == Guid.Empty)
            throw new ArgumentException("PurchaseId must be provided.", nameof(purchaseId));
        if (receivedById == Guid.Empty)
            throw new ArgumentException("ReceivedById must be provided.", nameof(receivedById));

        Id = id;
        PurchaseId = purchaseId;
        SupplierId = supplierId;
        ReceivedById = receivedById;
        ReceivedOn = receivedOn;
        DeliveryNoteNumber = deliveryNoteNumber;
        Remarks = remarks;
        Status = GoodsReceiptStatus.Draft;

        QueueDomainEvent(new GoodsReceiptCreated { GoodsReceipt = this });
    }

    public static GoodsReceipt Create(
        Guid purchaseId,
        Guid? supplierId,
        Guid receivedById,
        DateTime? receivedOn,
        string? deliveryNoteNumber,
        string? remarks)
    {
        return new GoodsReceipt(
            Guid.NewGuid(),
            purchaseId,
            supplierId,
            receivedById,
            receivedOn ?? DateTime.UtcNow,
            deliveryNoteNumber,
            remarks);
    }

    public GoodsReceipt UpdateHeader(Guid? supplierId, Guid receivedById, DateTime receivedOn, string? deliveryNoteNumber, string? remarks)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot modify a posted goods receipt.");

        bool updated = false;

        if (SupplierId != supplierId)
        {
            SupplierId = supplierId;
            updated = true;
        }

        if (ReceivedById != receivedById)
        {
            if (receivedById == Guid.Empty)
                throw new ArgumentException("ReceivedById must be provided.", nameof(receivedById));

            ReceivedById = receivedById;
            updated = true;
        }

        if (ReceivedOn != receivedOn)
        {
            ReceivedOn = receivedOn;
            updated = true;
        }

        if (DeliveryNoteNumber != deliveryNoteNumber)
        {
            DeliveryNoteNumber = deliveryNoteNumber;
            updated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            updated = true;
        }

        if (updated)
        {
            QueueDomainEvent(new GoodsReceiptUpdated { GoodsReceiptId = Id });
        }

        return this;
    }

    public GoodsReceiptItem AddItem(Guid purchaseItemId, int qtyReceived, string? condition, string? remarks)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot add items to a posted goods receipt.");

        if (Items.Any(i => i.PurchaseItemId == purchaseItemId))
            throw new InvalidOperationException($"Receipt item for purchase item {purchaseItemId} already exists.");

        var item = GoodsReceiptItem.Create(Id, purchaseItemId, qtyReceived, condition, remarks);
        Items.Add(item);
        return item;
    }

    public void UpdateItem(Guid receiptItemId, int qtyReceived, string? condition, string? remarks)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot update items on a posted goods receipt.");

        var item = Items.FirstOrDefault(i => i.Id == receiptItemId);
        if (item is null)
            throw new InvalidOperationException($"Goods receipt item {receiptItemId} not found.");

        item.Update(qtyReceived, condition, remarks);
    }

    public void RemoveItem(Guid receiptItemId)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot remove items from a posted goods receipt.");

        var item = Items.FirstOrDefault(i => i.Id == receiptItemId);
        if (item is not null)
        {
            Items.Remove(item);
            QueueDomainEvent(new GoodsReceiptUpdated { GoodsReceiptId = Id });
        }
    }

    public void Post()
    {
        if (IsPosted)
            return;

        if (Items.Count == 0)
            throw new InvalidOperationException("Cannot post a goods receipt without items.");

        Status = GoodsReceiptStatus.Posted;
        QueueDomainEvent(new GoodsReceiptPosted { GoodsReceiptId = Id, PurchaseId = PurchaseId });
    }

    public void Cancel(string? reason = null)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot cancel a posted goods receipt.");

        if (Status == GoodsReceiptStatus.Cancelled)
            return;

        Status = GoodsReceiptStatus.Cancelled;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Remarks = string.IsNullOrWhiteSpace(Remarks) ? reason : $"{Remarks}\nCancellation: {reason}";
        }

        QueueDomainEvent(new GoodsReceiptUpdated { GoodsReceiptId = Id });
    }
}

