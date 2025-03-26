using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
public sealed record UpdatePurchaseItemCommand(
    Guid Id,
    Guid PurchaseId,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status = "Pending") : IRequest<UpdatePurchaseItemResponse>;
