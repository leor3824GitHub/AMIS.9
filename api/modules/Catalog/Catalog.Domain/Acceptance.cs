using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class Acceptance : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid SupplyOfficerId { get; private set; } // Accountable Employee ID
    public Guid? InspectionId { get; private set; }
    public DateTime AcceptanceDate { get; private set; }
    public string? Remarks { get; private set; }
    public bool IsPosted { get; private set; }
    public DateTime? PostedOn { get; private set; }
    public AcceptanceStatus Status { get; private set; }

    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Employee SupplyOfficer { get; private set; } = default!;
    public virtual ICollection<AcceptanceItem> Items { get; private set; } = [];

    public bool IsFullAcceptance => Items.All(i => i.PurchaseItem.Qty == i.QtyAccepted);
    private Acceptance() { }

    private Acceptance(Guid id, Guid purchaseId, Guid supplyOfficerId, Guid? inspectionId, DateTime acceptanceDate, string? remarks)
    {
        Id = id;
        PurchaseId = purchaseId;
        SupplyOfficerId = supplyOfficerId;
        InspectionId = inspectionId;
        AcceptanceDate = acceptanceDate;
        Remarks = remarks;
        Status = AcceptanceStatus.Pending;

        QueueDomainEvent(new AcceptanceCreated { Acceptance = this });
    }

    public static Acceptance Create(Guid purchaseId, Guid supplyOfficerId, DateTime acceptanceDate, string? remarks, Guid? inspectionId = null)
    {
        return new Acceptance(Guid.NewGuid(), purchaseId, supplyOfficerId, inspectionId, acceptanceDate, remarks);
    }

    public Acceptance Update(Guid supplyOfficerId, DateTime acceptanceDate, string? remarks)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot modify a posted acceptance.");
        }

        bool isUpdated = false;

        if (SupplyOfficerId != supplyOfficerId)
        {
            SupplyOfficerId = supplyOfficerId;
            isUpdated = true;
        }

        if (AcceptanceDate != acceptanceDate)
        {
            AcceptanceDate = acceptanceDate;
            isUpdated = true;
        }

        if (Remarks != remarks)
        {
            Remarks = remarks;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new AcceptanceUpdated { Acceptance = this });
        }

        return this;
    }

    public void AddItem(Guid purchaseItemId, int qtyAccepted, string? remarks)
    {
        if (IsPosted)
        {
            throw new InvalidOperationException("Cannot add items to a posted acceptance.");
        }

        if (qtyAccepted <= 0)
        {
            throw new ArgumentException("Accepted quantity must be greater than zero.", nameof(qtyAccepted));
        }

        var item = AcceptanceItem.Create(Id, purchaseItemId, qtyAccepted, remarks);
        Items.Add(item);
    }

    public void LinkInspection(Guid inspectionId)
    {
        if (inspectionId == Guid.Empty)
        {
            throw new ArgumentException("InspectionId must be provided.", nameof(inspectionId));
        }

        if (InspectionId.HasValue && InspectionId.Value != inspectionId)
        {
            throw new InvalidOperationException("Acceptance is already linked to a different inspection.");
        }

        InspectionId = inspectionId;
    }

    public void PostAcceptance(DateTime? postedOnUtc = null)
    {
        if (IsPosted)
        {
            return;
        }

        if (Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot post an acceptance without any items.");
        }

        IsPosted = true;
        PostedOn = postedOnUtc ?? DateTime.UtcNow;
        Status = AcceptanceStatus.Posted;

        QueueDomainEvent(new AcceptancePosted { AcceptanceId = Id });
    }
}
