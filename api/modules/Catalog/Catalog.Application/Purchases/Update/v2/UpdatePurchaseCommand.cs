using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v2;

public sealed record PurchaseItemDto(
    Guid? Id,                 // The ID of the purchase item (could be null for new items)
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status = "Pending"
);

public sealed record UpdatePurchaseCommand(
    Guid Id,           // The ID of the purchase to update
    Guid? SupplierId,
    DateTime? PurchaseDate,
    [property: DefaultValue(0)] decimal TotalAmount = 0,
    [property: DefaultValue("InProgress")] string Status = "InProgress",
    ICollection<PurchaseItemDto>? Items = null
) : IRequest<UpdatePurchaseResponse>;
