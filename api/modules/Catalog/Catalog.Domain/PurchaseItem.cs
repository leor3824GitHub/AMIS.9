using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class PurchaseItem : AuditableEntity
{
    public Guid? PurchaseId { get; private set; }
    public Guid? ProductId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public PurchaseStatus? ItemStatus { get; private set; }
    public PurchaseItemInspectionStatus? InspectionStatus { get; private set; }
    public PurchaseItemAcceptanceStatus? AcceptanceStatus { get; private set; }

    // Convenience computed value for projections and totals
    public decimal LineTotal => Qty * UnitPrice;

    // New aggregate fields for summary views
    public int? QtyInspected { get; private set; }  // Set from InspectionItem
    public int? QtyPassed { get; private set; }
    public int? QtyFailed { get; private set; }

    public int? QtyAccepted { get; private set; } // Set from AcceptanceItem

    // Navigation
    public virtual Product Product { get; private set; } = default!;
    public virtual ICollection<InspectionItem> InspectionItems { get; private set; } = [];
    public virtual ICollection<AcceptanceItem> AcceptanceItems { get; private set; } = [];

    private PurchaseItem() { }

    private PurchaseItem(Guid id, Guid? purchaseId, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemstatus)
    {
        Id = id;
        PurchaseId = purchaseId;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
        ItemStatus = itemstatus;

        QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = this });
    }

    public static PurchaseItem Create(Guid? purchaseId, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemstatus)
    {
        return new PurchaseItem(Guid.NewGuid(), purchaseId, productId, qty, unitPrice, itemstatus);
    }
    
    public void UpdateQuantity(int newQty)
    {
        if (newQty <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(newQty));
        if (Qty != newQty)
        {
            Qty = newQty;
            QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = this });
        }
    }
        public void UpdateInspectionSummary(int inspected, int passed, int failed)
    {
        QtyInspected = inspected;
        QtyPassed = passed;
        QtyFailed = failed;
    }

    public void UpdateAcceptanceSummary(int accepted)
    {
        QtyAccepted = accepted;
    }

    public PurchaseItem Update(Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemstatus)
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
    public PurchaseItem UpdateInspectionStatus(PurchaseItemInspectionStatus status)
    {
        if (InspectionStatus != status)
        {
            InspectionStatus = status;
            QueueDomainEvent(new PurchaseItemInspected { PurchaseItem = this });
        }

        return this;
    }

    public PurchaseItem UpdateAcceptanceStatus(PurchaseItemAcceptanceStatus status)
    {
        if (AcceptanceStatus != status)
        {
            AcceptanceStatus = status;
            QueueDomainEvent(new PurchaseItemAccepted { PurchaseItem = this });
        }

        return this;
    }
}

