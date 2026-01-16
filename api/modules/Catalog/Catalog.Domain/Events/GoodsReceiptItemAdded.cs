using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record GoodsReceiptItemAdded : DomainEvent
{
    public GoodsReceiptItem GoodsReceiptItem { get; init; } = default!;
}
