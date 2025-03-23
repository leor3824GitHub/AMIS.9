using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record ProductCreated : DomainEvent
{
    public Product? Product { get; set; }
}
