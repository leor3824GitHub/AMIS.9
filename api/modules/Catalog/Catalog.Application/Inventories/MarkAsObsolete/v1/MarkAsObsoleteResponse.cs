using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inventories.MarkAsObsolete.v1;

public sealed record MarkAsObsoleteResponse(
    Guid InventoryId,
    StockStatus Status,
    string Message
);
