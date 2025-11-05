namespace AMIS.WebApi.Catalog.Application.Inventories.ReserveStock.v1;

public sealed record ReserveStockResponse(
    Guid InventoryId,
    int ReservedQuantity,
    int AvailableQuantity,
    string Message);
