using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record AcceptanceItemCreated : DomainEvent
{
    public AcceptanceItem? AcceptanceItem { get; set; }
}
