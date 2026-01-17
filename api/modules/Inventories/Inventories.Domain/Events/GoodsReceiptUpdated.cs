using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record GoodsReceiptUpdated : DomainEvent
{
    public Guid GoodsReceiptId { get; init; }
}

