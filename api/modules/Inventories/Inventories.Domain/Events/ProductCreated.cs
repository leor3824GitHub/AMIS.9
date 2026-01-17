using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record ProductCreated : DomainEvent
{
    public Product? Product { get; set; }
}

