using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.Inventories.Update.v1;
public sealed record UpdateInventoryCommand(
    Guid Id,
    Guid ProductId,
    decimal Qty,
    decimal AvePrice,
    string? Location) : IRequest<UpdateInventoryResponse>;
