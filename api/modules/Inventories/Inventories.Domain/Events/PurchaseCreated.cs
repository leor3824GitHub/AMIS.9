using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record PurchaseCreated : DomainEvent
{
    public Purchase? Purchase { get; set; }
}

