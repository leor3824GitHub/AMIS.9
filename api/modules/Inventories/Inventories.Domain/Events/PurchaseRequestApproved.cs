using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record PurchaseRequestApproved : DomainEvent
{
    public Guid PurchaseRequestId { get; set; }
}

