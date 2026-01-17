using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record IssuanceAccepted : DomainEvent
{
    public Issuance? Issuance { get; set; }
}

public sealed record IssuanceRejected : DomainEvent
{
    public Issuance? Issuance { get; set; }
}

public sealed record IssuanceCancelled : DomainEvent
{
    public Issuance? Issuance { get; set; }
}

public sealed record IssuanceReturned : DomainEvent
{
    public Issuance? Issuance { get; set; }
}

