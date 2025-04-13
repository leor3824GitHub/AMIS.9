﻿using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public sealed record PurchaseItemDto(
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus
);

public sealed record CreatePurchaseCommand(
    Guid? SupplierId,
    DateTime? PurchaseDate,
    PurchaseStatus? Status,
    [property: DefaultValue(0)] decimal TotalAmount = 0,    
    ICollection<PurchaseItemDto>? Items = null
) : IRequest<CreatePurchaseResponse>;

