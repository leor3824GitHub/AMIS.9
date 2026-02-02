using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents an audit log entry for semi-expendable inventory transactions
/// Tracks all changes to semi-expendable inventory for compliance and reconciliation purposes
/// </summary>
public class SemexTransactionLog : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Item code being transacted
    /// </summary>
    public string ItemCode { get; private set; } = string.Empty;

    /// <summary>
    /// Type of transaction (SMRR for receiving, SMIR for issuance)
    /// </summary>
    public string TransactionType { get; private set; } = string.Empty;

    /// <summary>
    /// Report number that triggered this transaction
    /// </summary>
    public string ReportNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Quantity change (positive for additions, negative for deductions)
    /// </summary>
    public int QuantityChange { get; private set; }

    /// <summary>
    /// Quantity before the transaction
    /// </summary>
    public int InventoryBefore { get; private set; }

    /// <summary>
    /// Quantity after the transaction
    /// </summary>
    public int InventoryAfter { get; private set; }

    /// <summary>
    /// Item status before the transaction
    /// </summary>
    public InventoryItemStatus StatusBefore { get; private set; }

    /// <summary>
    /// Item status after the transaction
    /// </summary>
    public InventoryItemStatus StatusAfter { get; private set; }

    /// <summary>
    /// Indicates whether the transaction was successful
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// Error message if transaction failed
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// User who initiated the transaction
    /// </summary>
    public string? InitiatedBy { get; private set; }

    /// <summary>
    /// Date and time of the transaction
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// Correlation ID to link related transactions across reports
    /// Enables tracing of an asset lifecycle: Receive → Assign → Return
    /// </summary>
    public string? CorrelationId { get; private set; }

    private SemexTransactionLog() { }

    /// <summary>
    /// Creates a successful transaction log entry
    /// </summary>
    public static SemexTransactionLog CreateSuccess(
        string itemCode,
        string transactionType,
        string reportNumber,
        int quantityChange,
        int inventoryBefore,
        int inventoryAfter,
        InventoryItemStatus statusBefore,
        InventoryItemStatus statusAfter,
        string? initiatedBy = null,
        string? correlationId = null)
    {
        ArgumentNullException.ThrowIfNull(itemCode);
        ArgumentNullException.ThrowIfNull(transactionType);
        ArgumentNullException.ThrowIfNull(reportNumber);

        return new SemexTransactionLog
        {
            ItemCode = itemCode,
            TransactionType = transactionType,
            ReportNumber = reportNumber,
            QuantityChange = quantityChange,
            InventoryBefore = inventoryBefore,
            InventoryAfter = inventoryAfter,
            StatusBefore = statusBefore,
            StatusAfter = statusAfter,
            Success = true,
            ErrorMessage = null,
            InitiatedBy = initiatedBy,
            CorrelationId = correlationId,
            TransactionDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a failed transaction log entry
    /// </summary>
    public static SemexTransactionLog CreateFailure(
        string itemCode,
        string transactionType,
        string reportNumber,
        int inventoryBefore,
        InventoryItemStatus statusBefore,
        string errorMessage,
        string? initiatedBy = null,
        string? correlationId = null)
    {
        ArgumentNullException.ThrowIfNull(itemCode);
        ArgumentNullException.ThrowIfNull(transactionType);
        ArgumentNullException.ThrowIfNull(reportNumber);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return new SemexTransactionLog
        {
            ItemCode = itemCode,
            TransactionType = transactionType,
            ReportNumber = reportNumber,
            QuantityChange = 0,
            InventoryBefore = inventoryBefore,
            InventoryAfter = inventoryBefore, // No change on failure
            StatusBefore = statusBefore,
            StatusAfter = statusBefore, // No change on failure
            Success = false,
            ErrorMessage = errorMessage,
            InitiatedBy = initiatedBy,
            CorrelationId = correlationId,
            TransactionDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Gets the net balance after transaction
    /// </summary>
    public int GetNetBalance() => InventoryAfter;

    /// <summary>
    /// Gets transaction summary as a readable string
    /// </summary>
    public string GetSummary() =>
        $"[{TransactionDate:yyyy-MM-dd HH:mm:ss}] {TransactionType} {ReportNumber}: {ItemCode} {(QuantityChange > 0 ? "+" : "")}{QuantityChange} units ({InventoryBefore} → {InventoryAfter})";
}
