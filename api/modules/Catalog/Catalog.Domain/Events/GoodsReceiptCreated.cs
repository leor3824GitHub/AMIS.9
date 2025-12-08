using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record GoodsReceiptCreated : DomainEvent
{
    public GoodsReceipt GoodsReceipt { get; init; } = default!;
}
