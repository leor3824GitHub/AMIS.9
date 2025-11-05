using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inventories.SetCostingMethod.v1;

public sealed record SetCostingMethodResponse(
    Guid InventoryId,
    CostingMethod Method,
    string Message
);
