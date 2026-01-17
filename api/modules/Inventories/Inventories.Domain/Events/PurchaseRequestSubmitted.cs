using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record PurchaseRequestSubmitted : DomainEvent
{
    public Guid PurchaseRequestId { get; set; }
}

