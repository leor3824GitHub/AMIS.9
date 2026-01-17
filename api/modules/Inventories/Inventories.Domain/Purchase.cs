using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

public class Purchase : AuditableEntity, IAggregateRoot
{
    public Guid? PurchaseRequestId { get; private set; }
    public Guid? SelectedCanvassId { get; private set; }
    public Guid? SupplierId { get; private set; }
    public DateTime? PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; } = 0;
    public PurchaseStatus Status { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? Remarks { get; private set; }
    public string? DeliveryAddress { get; private set; }
    
    public virtual Supplier? Supplier { get; init; }
    public virtual PurchaseRequest? PurchaseRequest { get; init; }
    public virtual Canvass? SelectedCanvass { get; init; }
    public virtual ICollection<PurchaseItem> Items { get; private set; } = [];
    public virtual ICollection<Inspection> Inspections { get; private set; } = [];
    public virtual ICollection<Acceptance> Acceptances { get; private set; } = [];
    public virtual ICollection<GoodsReceipt> GoodsReceipts { get; private set; } = [];

    // Computed properties - not persisted
    public bool HasItems => Items.Count > 0;
    public int TotalItemsCount => Items.Sum(i => i.Qty);
    public bool IsFullyInspected => Items.Count > 0 && Items.All(i => 
        i.InspectionStatus == PurchaseItemInspectionStatus.Passed || 
        i.InspectionStatus == PurchaseItemInspectionStatus.Failed);
    public bool IsFullyAccepted => Items.Count > 0 && Items.All(i => 
        i.AcceptanceStatus == PurchaseItemAcceptanceStatus.Accepted);
    public bool IsPartiallyInspected => Items.Any(i => 
        i.InspectionStatus != PurchaseItemInspectionStatus.NotInspected);
    public bool IsPartiallyAccepted => Items.Any(i => 
        i.AcceptanceStatus == PurchaseItemAcceptanceStatus.PartiallyAccepted || 
        i.AcceptanceStatus == PurchaseItemAcceptanceStatus.Accepted);

    private Purchase() { }

    private Purchase(Guid id, Guid? purchaseRequestId, Guid? selectedCanvassId, Guid? supplierId, DateTime? purchaseDate, PurchaseStatus status, string? referenceNumber, string? remarks, string? deliveryAddress)
    {
        Id = id;
        PurchaseRequestId = purchaseRequestId;
        SelectedCanvassId = selectedCanvassId;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate ?? DateTime.UtcNow;
        Status = status;
        ReferenceNumber = referenceNumber;
        Remarks = remarks;
        DeliveryAddress = deliveryAddress;
        TotalAmount = 0;

        QueueDomainEvent(new PurchaseCreated { Purchase = this });
    }

    public static Purchase Create(Guid? supplierId, DateTime? purchaseDate = null, string? referenceNumber = null, string? remarks = null, string? deliveryAddress = null, Guid? purchaseRequestId = null, Guid? selectedCanvassId = null)
    {
        var purchase = new Purchase(Guid.NewGuid(), purchaseRequestId, selectedCanvassId, supplierId, purchaseDate, PurchaseStatus.Draft, referenceNumber, remarks, deliveryAddress);
        return purchase;
    }

    public Purchase Update(Guid? supplierId, DateTime? purchaseDate, string? referenceNumber, string? remarks, string? deliveryAddress)
    {
        if (Status == PurchaseStatus.Closed || Status == PurchaseStatus.Cancelled)
        {
            throw new InvalidOperationException($"Cannot modify a {Status} purchase.");
        }

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

        if (ReferenceNumber != referenceNumber)
        {
            ReferenceNumber = referenceNumber;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (DeliveryAddress != deliveryAddress)
        {
            DeliveryAddress = deliveryAddress;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PurchaseUpdated { Purchase = this });
        }

        return this;
    }

    public void LinkToPurchaseRequest(Guid purchaseRequestId)
    {
        if (purchaseRequestId == Guid.Empty)
            throw new ArgumentException("PurchaseRequestId must be provided.", nameof(purchaseRequestId));

        if (Status != PurchaseStatus.Draft)
            throw new InvalidOperationException("Can only link a purchase request while purchase is in Draft.");

        if (PurchaseRequestId.HasValue && PurchaseRequestId != purchaseRequestId)
            throw new InvalidOperationException("Purchase is already linked to a different purchase request.");

        PurchaseRequestId = purchaseRequestId;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void SelectCanvass(Guid canvassId)
    {
        if (canvassId == Guid.Empty)
            throw new ArgumentException("CanvassId must be provided.", nameof(canvassId));

        if (Status != PurchaseStatus.Draft)
            throw new InvalidOperationException("Can only select canvass while purchase is in Draft.");

        if (SelectedCanvassId.HasValue && SelectedCanvassId != canvassId)
            throw new InvalidOperationException("A different canvass is already selected for this purchase.");

        SelectedCanvassId = canvassId;
        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    public void AddItem(Guid? productId, int qty, decimal unitPrice, PurchaseStatus? itemStatus = null)
    {
        if (Status == PurchaseStatus.Closed || Status == PurchaseStatus.Cancelled)
        {
            throw new InvalidOperationException($"Cannot add items to a {Status} purchase.");
        }

        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        }

        var item = PurchaseItem.Create(this.Id, productId, qty, unitPrice, itemStatus ?? PurchaseStatus.Draft);
        Items.Add(item);
        RecalculateTotalAmount();
    }

    public void AddItem(Guid id, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        if (Status == PurchaseStatus.Closed || Status == PurchaseStatus.Cancelled)
        {
            throw new InvalidOperationException($"Cannot add items to a {Status} purchase.");
        }

        var item = PurchaseItem.Create(id, this.Id, productId, qty, unitPrice, status);
        Items.Add(item);
        RecalculateTotalAmount();
    }

    public void UpdateItem(Guid itemId, Guid? productId, int qty, decimal unitPrice, PurchaseStatus? status)
    {
        if (Status == PurchaseStatus.Closed || Status == PurchaseStatus.Cancelled)
        {
            throw new InvalidOperationException($"Cannot update items in a {Status} purchase.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
        {
            throw new InvalidOperationException($"Purchase item with ID {itemId} not found.");
        }

        item.Update(productId, qty, unitPrice, status);
        RecalculateTotalAmount();
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status == PurchaseStatus.Closed || Status == PurchaseStatus.Cancelled)
        {
            throw new InvalidOperationException($"Cannot remove items from a {Status} purchase.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is not null)
        {
            Items.Remove(item);
            RecalculateTotalAmount();
        }
    }

    public void Submit()
    {
        if (Status != PurchaseStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot submit a purchase with status {Status}.");
        }

        if (Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot submit a purchase without items.");
        }

        ChangeStatus(PurchaseStatus.Submitted);
    }

    public void MarkAsDelivered()
    {
        if (Status != PurchaseStatus.Submitted && Status != PurchaseStatus.PartiallyDelivered)
        {
            throw new InvalidOperationException($"Cannot mark as delivered from status {Status}.");
        }

        ChangeStatus(PurchaseStatus.Delivered);
    }

    public void MarkAsPartiallyDelivered()
    {
        if (Status != PurchaseStatus.Submitted)
        {
            throw new InvalidOperationException($"Cannot mark as partially delivered from status {Status}.");
        }

        ChangeStatus(PurchaseStatus.PartiallyDelivered);
    }

    public void Close()
    {
        if (Status != PurchaseStatus.Delivered)
        {
            throw new InvalidOperationException($"Cannot close a purchase with status {Status}. Must be Delivered first.");
        }

        if (!IsFullyInspected)
        {
            throw new InvalidOperationException("Cannot close a purchase that is not fully inspected.");
        }

        if (!IsFullyAccepted)
        {
            throw new InvalidOperationException("Cannot close a purchase that is not fully accepted.");
        }

        ChangeStatus(PurchaseStatus.Closed);
    }

    public void Cancel(string? reason = null)
    {
        if (Status == PurchaseStatus.Closed)
        {
            throw new InvalidOperationException("Cannot cancel a closed purchase.");
        }

        if (Status == PurchaseStatus.Cancelled)
        {
            return; // Already cancelled
        }

        if (IsPartiallyAccepted)
        {
            throw new InvalidOperationException("Cannot cancel a purchase that has already been partially accepted.");
        }

        Remarks = string.IsNullOrWhiteSpace(reason) ? Remarks : $"{Remarks}\nCancellation Reason: {reason}";
        ChangeStatus(PurchaseStatus.Cancelled);
    }

    private void ChangeStatus(PurchaseStatus newStatus)
    {
        if (Status == newStatus) return;

        if (!IsValidTransition(Status, newStatus))
        {
            throw new InvalidOperationException($"Cannot transition purchase from {Status} to {newStatus}.");
        }

        Status = newStatus;

        QueueDomainEvent(new PurchaseUpdated { Purchase = this });
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.Qty * i.UnitPrice);
    }

    private static bool IsValidTransition(PurchaseStatus current, PurchaseStatus next)
    {
        var validTransitions = new Dictionary<PurchaseStatus, PurchaseStatus[]>
        {
            { PurchaseStatus.Draft, new[] { PurchaseStatus.Submitted, PurchaseStatus.Cancelled } },
            { PurchaseStatus.Submitted, new[] { PurchaseStatus.PartiallyDelivered, PurchaseStatus.Delivered, PurchaseStatus.Cancelled } },
            { PurchaseStatus.PartiallyDelivered, new[] { PurchaseStatus.Delivered, PurchaseStatus.Cancelled } },
            { PurchaseStatus.Delivered, new[] { PurchaseStatus.Closed } },
            { PurchaseStatus.Closed, Array.Empty<PurchaseStatus>() },
            { PurchaseStatus.Cancelled, Array.Empty<PurchaseStatus>() }
        };

        return validTransitions.TryGetValue(current, out var allowed) && allowed.Contains(next);
    }
}


