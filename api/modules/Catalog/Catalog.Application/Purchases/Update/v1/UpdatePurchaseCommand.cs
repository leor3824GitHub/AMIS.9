using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public sealed record UpdatePurchaseCommand(
    Guid Id,
    Guid SupplierId,
    DateTime PurchaseDate,
    decimal TotalAmount = 0,
    string? Status = "Pending" //Pending', 'Received', 'Cancelled
    ) : IRequest<UpdatePurchaseResponse>;
