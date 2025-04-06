using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public sealed record UpdatePurchaseCommand(
    Guid Id,
    Guid? SupplierId,
    DateTime PurchaseDate,
    string Status,
    decimal TotalAmount = 0) : IRequest<UpdatePurchaseResponse>;
//In-progress', 'Partially', 'Cancelled
