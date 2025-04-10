using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public sealed record PurchaseItemUpdateDto(
    Guid Id,
    Guid? PurchaseId,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status = "Pending"
);
public sealed record UpdatePurchaseCommand(
    Guid Id,
    Guid? SupplierId,
    DateTime? PurchaseDate,
    decimal TotalAmount = 0,
    ICollection<PurchaseItemUpdateDto>? Items = null,
    string? Status = "InProgress"
) : IRequest<UpdatePurchaseResponse>;
