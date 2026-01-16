using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public sealed record PurchaseItemDto(
    Guid? ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus
);

public sealed record CreatePurchaseCommand(
    Guid? SupplierId,
    DateTime? PurchaseDate,
    PurchaseStatus? Status,
    ICollection<PurchaseItemDto>? Items = null,
    string? DeliveryAddress = null
) : IRequest<CreatePurchaseResponse>;

