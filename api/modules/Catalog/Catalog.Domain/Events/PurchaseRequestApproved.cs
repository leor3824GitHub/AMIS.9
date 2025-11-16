using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record PurchaseRequestApproved : DomainEvent
{
    public Guid PurchaseRequestId { get; set; }
}
