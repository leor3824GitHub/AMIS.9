using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record GoodsReceiptPosted : DomainEvent
{
    public Guid GoodsReceiptId { get; init; }
    public Guid PurchaseId { get; init; }
}

