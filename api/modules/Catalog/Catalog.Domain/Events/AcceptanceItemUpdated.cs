using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record AcceptanceItemUpdated : DomainEvent
{
    public AcceptanceItem? AcceptanceItem { get; set; }
}
