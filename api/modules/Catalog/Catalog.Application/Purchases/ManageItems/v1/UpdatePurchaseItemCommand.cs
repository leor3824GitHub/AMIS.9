using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;

public sealed record UpdatePurchaseItemCommand(
    Guid PurchaseId,
    Guid ItemId,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus) : IRequest;
