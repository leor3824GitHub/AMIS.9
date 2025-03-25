using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record IssuanceUpdated : DomainEvent
{
    public Issuance? Issuance { get; set; }
}
