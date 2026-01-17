using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record PurchaseRequestCreated : DomainEvent
{
    public PurchaseRequest? PurchaseRequest { get; set; }
}

