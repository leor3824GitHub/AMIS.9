using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record PurchaseRequestRejected : DomainEvent
{
    public Guid PurchaseRequestId { get; set; }
    public string? Reason { get; set; }
}
