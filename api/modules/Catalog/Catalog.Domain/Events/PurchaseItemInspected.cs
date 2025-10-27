using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record PurchaseItemInspected : DomainEvent
{
    public PurchaseItem? PurchaseItem { get; set; }
}
