namespace AMIS.WebApi.Inventories.Application.SemexTransactionLog.Search.v1;

public sealed record SemexTransactionLogResponse(
    Guid Id,
    string ItemCode,
    string TransactionType,
    string ReportNumber,
    int QuantityChange,
    int InventoryBefore,
    int InventoryAfter,
    string StatusBefore,
    string StatusAfter,
    bool Success,
    string? ErrorMessage,
    string? InitiatedBy,
    DateTime TransactionDate);
