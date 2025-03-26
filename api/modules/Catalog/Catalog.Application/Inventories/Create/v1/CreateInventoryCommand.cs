using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.Create.v1;
public sealed record CreateInventoryCommand(
    Guid ProductId,
    [property: DefaultValue(0)] int Qty,
    [property: DefaultValue(0)] decimal AvePrice) : IRequest<CreateInventoryResponse>;
