using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record AcceptancePosted : DomainEvent
{
    public Guid AcceptanceId { get; init; }
}

