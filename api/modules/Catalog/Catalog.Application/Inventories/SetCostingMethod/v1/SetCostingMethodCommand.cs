using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.SetCostingMethod.v1;

public sealed record SetCostingMethodCommand(
    Guid InventoryId,
    CostingMethod Method
) : IRequest<SetCostingMethodResponse>;
