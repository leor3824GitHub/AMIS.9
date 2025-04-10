using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record PurchaseItemCreated : DomainEvent
{
    public PurchaseItem? PurchaseItem { get; set; }
}

