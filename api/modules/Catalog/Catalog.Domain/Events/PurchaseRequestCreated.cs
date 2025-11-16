using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record PurchaseRequestCreated : DomainEvent
{
    public PurchaseRequest? PurchaseRequest { get; set; }
}
