using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InventoryUpdated : DomainEvent
{
    public Inventory? Inventory { get; set; }
}
