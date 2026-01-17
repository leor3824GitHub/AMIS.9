using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InventoryTransactionUpdated : DomainEvent
{
    public InventoryTransaction? InventoryTransaction { get; set; }
}

