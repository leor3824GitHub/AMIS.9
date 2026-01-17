using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record AcceptanceItemUpdated : DomainEvent
{
    public AcceptanceItem? AcceptanceItem { get; set; }
}

