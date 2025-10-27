using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record AcceptanceCreated : DomainEvent
{
    public Acceptance? Acceptance { get; set; }
}
