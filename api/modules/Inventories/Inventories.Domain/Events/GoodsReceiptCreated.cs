using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record GoodsReceiptCreated : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = default!;
}

