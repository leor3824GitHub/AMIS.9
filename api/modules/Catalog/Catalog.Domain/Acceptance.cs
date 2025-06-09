using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class Acceptance : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid SupplyOfficerId { get; private set; } // Accountable Employee ID
    public DateTime AcceptanceDate { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Employee SupplyOfficer { get; private set; } = default!;
    public virtual ICollection<AcceptanceItem> Items { get; private set; } = [];

    public bool IsFullAcceptance => Items.All(i => i.PurchaseItem.Qty == i.QtyAccepted);
    private Acceptance() { }

    private Acceptance(Guid id, Guid purchaseId, Guid supplyOfficerId, DateTime acceptanceDate, string? remarks)
    {
        Id = id;
        PurchaseId = purchaseId;
        SupplyOfficerId = supplyOfficerId;
        AcceptanceDate = acceptanceDate;
        Remarks = remarks;

        QueueDomainEvent(new AcceptanceCreated { Acceptance = this });
    }

    public static Acceptance Create(Guid purchaseId, Guid supplyOfficerId, DateTime acceptanceDate, string? remarks)
    {
        return new Acceptance(Guid.NewGuid(), purchaseId, supplyOfficerId, acceptanceDate, remarks);
    }

    public Acceptance Update(Guid supplyOfficerId, DateTime acceptanceDate, string? remarks)
    {
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
        var item = AcceptanceItem.Create(Id, purchaseItemId, qtyAccepted, remarks);
        Items.Add(item);
    }
}
