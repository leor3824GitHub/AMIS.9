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

    public void AddItem(Guid productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        var item = PurchaseItem.Create(this.Id, productId, qty, unitPrice, status);
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
    public void SyncItems(List<PurchaseItemUpdate> items)
    {
        var existingMap = Items.ToDictionary(i => i.Id, i => i);
        var updatedItems = new List<PurchaseItem>();

        foreach (var update in items)
        {
            if (update.Id.HasValue && existingMap.TryGetValue(update.Id.Value, out var existing))
            {
                // Track state before update for event comparison (optional)
                var before = (existing.Qty, existing.UnitPrice, existing.ProductId, existing.ItemStatus);

                existing.Update(update.ProductId, update.Qty, update.UnitPrice, update.ItemStatus);
                updatedItems.Add(existing);

                if (before != (existing.Qty, existing.UnitPrice, existing.ProductId, existing.ItemStatus))
                {
                    QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = existing });
                }
            }
            else
            {
                var newItem = PurchaseItem.Create(Id, update.ProductId, update.Qty, update.UnitPrice, update.ItemStatus);
                updatedItems.Add(newItem);
                QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = newItem });
            }
        }

        // Remove old items not in the updated list
        var toRemove = Items.Where(i => !updatedItems.Any(u => u.Id == i.Id)).ToList();
        foreach (var item in toRemove)
        {
            Items.Remove(item);
            QueueDomainEvent(new PurchaseItemRemoved { PurchaseItem = item });
        }

        // Add new items to the collection
        foreach (var item in updatedItems)
        {
            if (!Items.Any(i => i.Id == item.Id))
                Items.Add(item);
        }

        // Update total based on current items
        var total = Items.Sum(i => i.Qty * i.UnitPrice);
        Update(SupplierId, PurchaseDate, total, Status);
    }

}

