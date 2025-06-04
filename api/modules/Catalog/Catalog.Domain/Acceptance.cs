using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class Acceptance : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid AcceptedBy { get; private set; } // Accountable Employee ID
    public DateTime AcceptanceDate { get; private set; }
    public string? Remarks { get; private set; }

    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual Employee AccountableOfficer { get; private set; } = default!;
    public virtual ICollection<AcceptanceItem> Items { get; private set; } = [];

    public bool IsFullAcceptance => Items.All(i => i.PurchaseItem.Qty == i.QtyAccepted);
    private Acceptance() { }

    private Acceptance(Guid id, Guid purchaseId, Guid acceptedBy, DateTime acceptanceDate, string? remarks)
    {
        Id = id;
        PurchaseId = purchaseId;
        AcceptedBy = acceptedBy;
        AcceptanceDate = acceptanceDate;
        Remarks = remarks;

        QueueDomainEvent(new AcceptanceCreated { Acceptance = this });
    }

    public static Acceptance Create(Guid purchaseId, Guid acceptedBy, DateTime acceptanceDate, string? remarks)
    {
        return new Acceptance(Guid.NewGuid(), purchaseId, acceptedBy, acceptanceDate, remarks);
    }

    public Acceptance Update(Guid acceptedBy, DateTime acceptanceDate, string? remarks)
    {
        bool isUpdated = false;

        if (AcceptedBy != acceptedBy)
        {
            AcceptedBy = acceptedBy;
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
