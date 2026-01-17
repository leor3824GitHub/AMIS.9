using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record IssuanceCreated : DomainEvent
{
    public Issuance? Issuance { get; set; }
}

