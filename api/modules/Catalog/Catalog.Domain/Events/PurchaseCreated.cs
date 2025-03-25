using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record PurchaseCreated : DomainEvent
{
    public Purchase? Purchase { get; set; }
}
