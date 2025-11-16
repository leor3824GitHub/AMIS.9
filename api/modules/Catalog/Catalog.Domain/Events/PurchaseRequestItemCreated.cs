using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record PurchaseRequestItemCreated : DomainEvent
{
    public PurchaseRequestItem? PurchaseRequestItem { get; set; }
}
