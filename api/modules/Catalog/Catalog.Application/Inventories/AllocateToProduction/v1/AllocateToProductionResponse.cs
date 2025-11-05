namespace AMIS.WebApi.Catalog.Application.Inventories.AllocateToProduction.v1;

public sealed record AllocateToProductionResponse(
    Guid InventoryId,
    int AllocatedQuantity,
    string Message
);
