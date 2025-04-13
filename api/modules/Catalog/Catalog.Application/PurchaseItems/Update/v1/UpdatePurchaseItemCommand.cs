using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
public sealed record UpdatePurchaseItemCommand(
    Guid Id,
    Guid PurchaseId,
    Guid? ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus) : IRequest<UpdatePurchaseItemResponse>;
