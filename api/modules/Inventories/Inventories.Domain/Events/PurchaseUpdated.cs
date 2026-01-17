using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record PurchaseUpdated : DomainEvent
{
    public Purchase? Purchase { get; set; }
}

