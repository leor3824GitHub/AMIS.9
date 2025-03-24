using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InventoryCreated : DomainEvent
{
    public Inventory? Inventory { get; set; }
}
