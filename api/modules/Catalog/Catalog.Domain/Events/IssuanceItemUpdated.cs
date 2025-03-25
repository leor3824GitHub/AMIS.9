using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record IssuanceItemUpdated : DomainEvent
{
    public IssuanceItem? IssuanceItem { get; set; }
}
