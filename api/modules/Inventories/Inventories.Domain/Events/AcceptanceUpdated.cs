using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record AcceptanceUpdated : DomainEvent
{
    public Acceptance? Acceptance { get; set; }
}

