using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;

public sealed record InventoryTransactionResponse(
    Guid? Id,
    Guid? ProductId,
    int Qty,
    decimal UnitCost,
    Guid? SourceId,
    string? location,
    TransactionType TransactionType
);
