using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InventoryTransactionCreated : DomainEvent
{
    public InventoryTransaction? InventoryTransaction { get; set; }
}
