using System.ComponentModel;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Purchases.ManageItems.v1;

public sealed record UpdatePurchaseItemCommand(
    Guid PurchaseId,
    Guid ItemId,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus) : IRequest;

