using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record IssuanceItemCreated : DomainEvent
{
    public IssuanceItem? IssuanceItem { get; set; }
}
