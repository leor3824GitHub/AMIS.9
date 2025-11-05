using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReleaseFromQuarantine.v1;

public sealed record ReleaseFromQuarantineResponse(
    Guid InventoryId,
    StockStatus Status,
    string Message
);
