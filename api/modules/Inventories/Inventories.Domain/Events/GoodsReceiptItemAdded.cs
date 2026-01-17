using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record GoodsReceiptItemAdded : DomainEvent
{
    public GoodsReceiptItem GoodsReceiptItem { get; init; } = default!;
}

