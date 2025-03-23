using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record CategoryUpdated : DomainEvent
{
    public Category? Category { get; set; }
}
