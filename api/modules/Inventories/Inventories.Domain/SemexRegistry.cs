using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the semi-expendable inventory registry that tracks the current state of all SEMEX (semi-expendable) items
/// This is the single source of truth for semi-expendable inventory quantities and status
/// </summary>
public class SemexRegistry : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique item identifier/code for the item
    /// </summary>
    public string ItemCode { get; private set; } = string.Empty;

    /// <summary>
    /// Description of the item
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Current quantity in inventory
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit of measurement
    /// </summary>
    public string Unit { get; private set; } = string.Empty;

    /// <summary>
    /// Location/store where item is kept
    /// </summary>
    public string Location { get; private set; } = string.Empty;

    /// <summary>
    /// Unit cost of the item
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Current status of the item
    /// </summary>
    public InventoryItemStatus Status { get; private set; } = InventoryItemStatus.NotReceived;

    /// <summary>
    /// Date when item was first received into inventory
    /// </summary>
    public DateTime ReceivedDate { get; private set; }

    /// <summary>
    /// Date when item was last issued out
    /// </summary>
    public DateTime? IssuedDate { get; private set; }

    /// <summary>
    /// Last transaction date (add or deduct)
    /// </summary>
    public DateTime LastTransactionDate { get; set; }

    /// <summary>
    /// Type of last transaction (SMRR = Receiving, SMIR = Issuance)
    /// </summary>
    public string LastTransactionType { get; set; } = string.Empty;

    /// <summary>
    /// Report number that triggered the last transaction
    /// </summary>
    public string LastTransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Optimistic concurrency token for safe updates
    /// </summary>
    [System.ComponentModel.DataAnnotations.Timestamp]
    public byte[]? RowVersion { get; set; }

    private SemexRegistry() { }

    /// <summary>
    /// Creates a new semi-expendable item from a Supplies and Materials Receiving Report (SMRR)
    /// </summary>
    public static SemexRegistry CreateFromReceiving(
        string itemCode,
        string description,
        int quantity,
        string unit,
        string location,
        decimal unitCost,
        string reportNumber)
    {
        ArgumentNullException.ThrowIfNull(itemCode);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(unit);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(reportNumber);

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));

        // Normalize key fields to canonical forms to keep lookups consistent
        var normalizedCode = itemCode.Trim().ToUpperInvariant();
        var normalizedDesc = description.Trim();
        var normalizedUnit = unit.Trim();
        var normalizedLocation = location.Trim();

        return new SemexRegistry
        {
            ItemCode = normalizedCode,
            Description = normalizedDesc,
            Quantity = quantity,
            Unit = normalizedUnit,
            Location = normalizedLocation,
            UnitCost = unitCost,
            Status = InventoryItemStatus.InStock,
            ReceivedDate = DateTime.UtcNow,
            LastTransactionDate = DateTime.UtcNow,
            LastTransactionType = "SMRR",
            LastTransactionReference = reportNumber
        };
    }

    /// <summary>
    /// Adds quantity to an existing semi-expendable inventory item
    /// </summary>
    public void AddQuantity(int quantity, string reportNumber)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        ArgumentNullException.ThrowIfNull(reportNumber);

        Quantity += quantity;
        Status = InventoryItemStatus.InStock;
        LastTransactionDate = DateTime.UtcNow;
        LastTransactionType = "SMRR";
        LastTransactionReference = reportNumber;
    }

    /// <summary>
    /// Deducts quantity from semi-expendable inventory with validation
    /// </summary>
    public void DeductQuantity(int quantity, string reportNumber)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        ArgumentNullException.ThrowIfNull(reportNumber);

        // Validate current status
        if (Status != InventoryItemStatus.InStock)
            throw new InvalidOperationException(
                $"Cannot issue item with status '{Status}'. Only items with status '{InventoryItemStatus.InStock}' can be issued.");

        // Validate sufficient quantity
        if (Quantity < quantity)
            throw new InvalidOperationException(
                $"Insufficient inventory for ItemCode '{ItemCode}'. " +
                $"Available: {Quantity}, Requested: {quantity}");

        Quantity -= quantity;

        // Update status based on remaining quantity
        if (Quantity == 0)
            Status = InventoryItemStatus.Issued;

        LastTransactionDate = DateTime.UtcNow;
        LastTransactionType = "SMIR";
        LastTransactionReference = reportNumber;
        IssuedDate = DateTime.UtcNow;
    }
}
