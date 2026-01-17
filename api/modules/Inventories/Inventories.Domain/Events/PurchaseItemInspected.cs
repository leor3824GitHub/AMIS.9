using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record PurchaseItemInspected : DomainEvent
{
    public PurchaseItem? PurchaseItem { get; set; }
}

