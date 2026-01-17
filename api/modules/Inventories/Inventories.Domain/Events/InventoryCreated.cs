using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InventoryCreated : DomainEvent
{
    public Inventory? Inventory { get; set; }
}

