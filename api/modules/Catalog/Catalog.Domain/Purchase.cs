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
        // Step 1: Map existing items by ID
        var existingMap = Items.ToDictionary(i => i.Id, i => i);
        var updatedItemIds = new HashSet<Guid>();
        var newItems = new List<PurchaseItem>();

        // Step 2: Process each item in the input
        foreach (var update in items)
        {
            if (update.Id.HasValue && existingMap.TryGetValue(update.Id.Value, out var existing))
            {
                // Save original state for comparison
                var before = (existing.ProductId, existing.Qty, existing.UnitPrice, existing.ItemStatus);

                // Update existing item
                existing.Update(update.ProductId, update.Qty, update.UnitPrice, update.ItemStatus);
                updatedItemIds.Add(existing.Id);

                // Raise event only if there was a change
                if (before != (existing.ProductId, existing.Qty, existing.UnitPrice, existing.ItemStatus))
                {
                    QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = existing });
                }
            }
            else
            {
                // Create new item
                var newItem = PurchaseItem.Create(Id, update.ProductId, update.Qty, update.UnitPrice, update.ItemStatus);

                // Add to temp collection only (do NOT modify Items yet)
                newItems.Add(newItem);
                updatedItemIds.Add(newItem.Id);

                // Raise domain event
                QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = newItem });
            }
        }

        // Step 3: Remove items not present in the update list
        var toRemove = Items.Where(i => !updatedItemIds.Contains(i.Id)).ToList();
        foreach (var item in toRemove)
        {
            Items.Remove(item);
            QueueDomainEvent(new PurchaseItemRemoved { PurchaseItem = item });
        }

        // Step 4: Now safely add new items to the collection
        foreach (var newItem in newItems)
        {
            Items.Add(newItem);
        }

        // Optional: Recalculate total if needed
        // var total = Items.Sum(i => i.Qty * i.UnitPrice);
        // Update(SupplierId, PurchaseDate, total, Status);
    }

}

