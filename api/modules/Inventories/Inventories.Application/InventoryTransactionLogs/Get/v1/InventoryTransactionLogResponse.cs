using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;

public sealed record InventoryTransactionLogResponse(
    Guid Id,
    string PropertyCode,
    string TransactionType,
    string ReportNumber,
    int QuantityChange,
    int InventoryBefore,
    int InventoryAfter,
    InventoryItemStatus StatusBefore,
    InventoryItemStatus StatusAfter,
    bool Success,
    string? ErrorMessage,
    string? InitiatedBy,
    DateTime TransactionDate);
