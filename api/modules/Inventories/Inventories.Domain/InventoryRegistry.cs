using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the inventory registry that tracks the current state of all PPE items
/// This is the single source of truth for inventory quantities and status
/// </summary>
public class InventoryRegistry : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique property code/identifier for the item
    /// </summary>
    public string PropertyCode { get; private set; } = string.Empty;

    /// <summary>
    /// Description of the item
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Current quantity in inventory
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Location/store where item is kept
    /// </summary>
    public string Location { get; private set; } = string.Empty;

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
    /// Type of last transaction (PPERR = Receiving, PPEIR = Issuance)
    /// </summary>
    public string LastTransactionType { get; set; } = string.Empty;

    /// <summary>
    /// Report number that triggered the last transaction
    /// </summary>
    public string LastTransactionReference { get; set; } = string.Empty;

    private InventoryRegistry() { }

    /// <summary>
    /// Creates a new inventory item from a PPE Receiving Report (PPERR)
    /// </summary>
    public static InventoryRegistry CreateFromReceiving(
        string propertyCode,
        string description,
        int quantity,
        string location,
        string reportNumber)
    {
        ArgumentNullException.ThrowIfNull(propertyCode);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(reportNumber);

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        return new InventoryRegistry
        {
            PropertyCode = propertyCode,
            Description = description,
            Quantity = quantity,
            Location = location,
            Status = InventoryItemStatus.InStock,
            ReceivedDate = DateTime.UtcNow,
            LastTransactionDate = DateTime.UtcNow,
            LastTransactionType = "PPERR",
            LastTransactionReference = reportNumber
        };
    }

    /// <summary>
    /// Adds quantity to an existing inventory item
    /// </summary>
    public void AddQuantity(int quantity, string reportNumber)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        ArgumentNullException.ThrowIfNull(reportNumber);

        Quantity += quantity;
        Status = InventoryItemStatus.InStock;
        LastTransactionDate = DateTime.UtcNow;
        LastTransactionType = "PPERR";
        LastTransactionReference = reportNumber;
    }

    /// <summary>
    /// Deducts quantity from inventory with validation
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
                $"Insufficient inventory for PropertyCode '{PropertyCode}'. " +
                $"Available: {Quantity}, Requested: {quantity}");

        Quantity -= quantity;

        // Update status based on remaining quantity
        if (Quantity == 0)
            Status = InventoryItemStatus.Issued;

        IssuedDate = DateTime.UtcNow;
        LastTransactionDate = DateTime.UtcNow;
        LastTransactionType = "PPEIR";
        LastTransactionReference = reportNumber;
    }

    /// <summary>
    /// Marks the item as damaged
    /// </summary>
    public void MarkAsDamaged(string reason)
    {
        ArgumentNullException.ThrowIfNull(reason);
        Status = InventoryItemStatus.Damaged;
        LastTransactionDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the item as lost
    /// </summary>
    public void MarkAsLost(string reason)
    {
        ArgumentNullException.ThrowIfNull(reason);
        Status = InventoryItemStatus.Lost;
        LastTransactionDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the item as in transit
    /// </summary>
    public void MarkAsInTransit()
    {
        Status = InventoryItemStatus.InTransit;
        LastTransactionDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if item can be issued
    /// </summary>
    public bool CanBeIssued()
    {
        return Status == InventoryItemStatus.InStock && Quantity > 0;
    }

    /// <summary>
    /// Gets summary of current item status
    /// </summary>
    public (string PropertyCode, string Description, int Quantity, InventoryItemStatus Status, string Location)
        GetSummary() => (PropertyCode, Description, Quantity, Status, Location);
}
