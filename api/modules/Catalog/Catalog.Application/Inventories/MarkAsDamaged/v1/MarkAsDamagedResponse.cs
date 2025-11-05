using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inventories.MarkAsDamaged.v1;

public sealed record MarkAsDamagedResponse(
    Guid InventoryId,
    StockStatus Status,
    string Message
);
