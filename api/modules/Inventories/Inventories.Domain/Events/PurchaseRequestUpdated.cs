using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record PurchaseRequestUpdated : DomainEvent
{
    public PurchaseRequest? PurchaseRequest { get; set; }
}

