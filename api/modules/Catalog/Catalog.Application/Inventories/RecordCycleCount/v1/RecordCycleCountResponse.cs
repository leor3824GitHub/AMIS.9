using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inventories.RecordCycleCount.v1;

public record RecordCycleCountResponse(
    Guid InventoryId,
    int PreviousQty,
    int CountedQty,
    int Variance,
    StockStatus StockStatus,
    DateTime CountDate,
    string Message
);
