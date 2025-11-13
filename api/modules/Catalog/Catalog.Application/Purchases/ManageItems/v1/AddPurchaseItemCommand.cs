using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;

public sealed record AddPurchaseItemCommand(
    Guid PurchaseId,
    Guid ProductId,
    [property: DefaultValue(1)] int Qty,
    [property: DefaultValue(0)] decimal UnitPrice,
    PurchaseStatus? ItemStatus) : IRequest<AddPurchaseItemResponse>;
