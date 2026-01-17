using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record PurchaseItemUpdated : DomainEvent
{
    public PurchaseItem? PurchaseItem { get; set; }
}

