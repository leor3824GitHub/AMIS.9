﻿using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Update.v1;

public sealed record UpdateInventoryTransactionCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue("00000000-0000-0000-0000-000000000001")] Guid? ProductId,
    [property: DefaultValue(1)] int Qty,
    [property: DefaultValue(100.00)] decimal UnitCost,
    [property: DefaultValue("00000000-0000-0000-0000-000000000002")] Guid? SourceId,
    [property: DefaultValue("Default Location")] string? Location,
    [property: DefaultValue(TransactionType.Issuance)] TransactionType TransactionType
) : IRequest<UpdateInventoryTransactionResponse>;
