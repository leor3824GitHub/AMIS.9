using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.Inventories.Update.v1;
public sealed record UpdateInventoryCommand(
    Guid Id,
    Guid ProductId,
    int Qty,
    decimal AvePrice) : IRequest<UpdateInventoryResponse>;
