using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;

public sealed record PurchaseItemUpdateDto(
    Guid? Id,                 // The ID of the purchase item (could be null for new items)
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status = "Pending"
);
public sealed record UpdatePurchaseCommand(
    Guid Id,           // The ID of the purchase to update
    Guid? SupplierId,          // The optional supplier ID (could be null)
    DateTime? PurchaseDate,    // The optional purchase date
    decimal TotalAmount,       // The total amount (could be recalculated)
    string? Status,            // The optional purchase status
    ICollection<PurchaseItemUpdateDto>? Items = null  // The list of items to update/add/remove
) : IRequest<UpdatePurchaseResponse>;
