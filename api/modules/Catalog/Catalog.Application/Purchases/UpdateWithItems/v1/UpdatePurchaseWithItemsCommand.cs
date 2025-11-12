using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1;

public sealed record UpdatePurchaseWithItemsCommand(
    Guid Id,
    Guid? SupplierId,
    DateTime? PurchaseDate,
    decimal TotalAmount,
    PurchaseStatus? Status,
    string? ReferenceNumber,
    string? Notes,
    string? Currency,
    IReadOnlyList<PurchaseItemUpsert> Items,
    IReadOnlyList<Guid>? DeletedItemIds
) : IRequest<UpdatePurchaseWithItemsResponse>;

public sealed record PurchaseItemUpsert(
    Guid? Id,
    Guid? ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus
);
