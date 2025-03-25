using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
public sealed record UpdatePurchaseItemCommand(
    Guid Id,
    Guid PurchaseId,
    Guid ProductId,
    decimal Qty,
    decimal UnitPrice,
    string? Status = "Pending") : IRequest<UpdatePurchaseItemResponse>;
