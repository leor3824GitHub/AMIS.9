using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v1;
public sealed record CreatePurchaseItemCommand(
    Guid PurchaseId,
    Guid ProductId,
    [property: DefaultValue(1)] decimal Qty,
    [property: DefaultValue(0)] decimal UnitPrice,
    [property: DefaultValue("Pending")] string? Status = "Pending") : IRequest<CreatePurchaseItemResponse>;
