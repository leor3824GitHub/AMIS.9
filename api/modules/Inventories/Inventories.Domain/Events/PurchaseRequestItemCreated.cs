using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record PurchaseRequestItemCreated : DomainEvent
{
    public PurchaseRequestItem? PurchaseRequestItem { get; set; }
}

