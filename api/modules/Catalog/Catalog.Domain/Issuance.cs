using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Issuance : AuditableEntity, IAggregateRoot
{
    public Guid? ProductId { get; private set; }
    public string? Location { get; private set; }
    public decimal Qty { get; private set; }
    public decimal AvePrice { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    private Issuance() { }

    private Issuance(Guid id, Guid? productId, string? location, decimal qty, decimal avePrice)
    {
        Id = id;
        ProductId = productId;
        Location = location;
        Qty = qty;
        AvePrice = avePrice;

        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public static Issuance Create(Guid? productId, string? location, decimal qty, decimal avePrice)
    {
        return new Issuance(Guid.NewGuid(), productId, location, qty, avePrice);
    }

    public Issuance Update(Guid? productId, string? location, decimal qty, decimal avePrice)
    {
        bool isUpdated = false;

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (!string.Equals(Location, location, StringComparison.OrdinalIgnoreCase))
        {
            Location = location;
            isUpdated = true;
        }

        if (Qty != qty)
        {
            Qty = qty;
            isUpdated = true;
        }

        if (AvePrice != avePrice)
        {
            AvePrice = avePrice;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new IssuanceUpdated { Issuance = this });
        }

        return this;
    }
}

