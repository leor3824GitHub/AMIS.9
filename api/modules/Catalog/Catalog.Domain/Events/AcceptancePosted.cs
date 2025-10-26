using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record AcceptancePosted : DomainEvent
{
    public Guid AcceptanceId { get; init; }
}
