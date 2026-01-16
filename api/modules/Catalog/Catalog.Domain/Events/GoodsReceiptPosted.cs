using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record GoodsReceiptPosted : DomainEvent
{
    public Guid GoodsReceiptId { get; init; }
    public Guid PurchaseId { get; init; }
}
