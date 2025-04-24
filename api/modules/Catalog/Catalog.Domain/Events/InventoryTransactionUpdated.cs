using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InventoryTransactionUpdated : DomainEvent
{
    public InventoryTransaction? InventoryTransaction { get; set; }
}
