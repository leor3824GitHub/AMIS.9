using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record CategoryUpdated : DomainEvent
{
    public Category? Category { get; set; }
}

