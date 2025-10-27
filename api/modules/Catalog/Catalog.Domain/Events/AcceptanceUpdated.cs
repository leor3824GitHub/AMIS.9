using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record AcceptanceUpdated : DomainEvent
{
    public Acceptance? Acceptance { get; set; }
}
