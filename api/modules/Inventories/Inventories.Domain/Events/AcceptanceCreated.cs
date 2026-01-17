using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record AcceptanceCreated : DomainEvent
{
    public Acceptance? Acceptance { get; set; }
}

