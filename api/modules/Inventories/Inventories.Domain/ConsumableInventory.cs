using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Consumable Inventory - Items used within 1 year (≤ ₱50,000)
/// RCA Account: 10501000 - Supplies and Materials Inventory
/// Expense Account: 50203010 - Supplies and Materials Expense
/// Required Document: RSMI (Requisition and Issue Slip)
/// </summary>
public class ConsumableInventory : AuditableEntity, IAggregateRoot
{
    public string StockNumber { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public string Description { get; private set; } = default!;
    public decimal UnitCost { get; private set; }
    public int Quantity { get; private set; }
    public string UnitOfMeasure { get; private set; } = default!;
    public decimal WeightedAverageCost { get; private set; }
    public string? Location { get; private set; }
    public int ReorderLevel { get; private set; }

    // Navigation
    public virtual Product Product { get; private set; } = default!;

    private ConsumableInventory() { }

    private ConsumableInventory(
        Guid id,
        string stockNumber,
        Guid productId,
        string description,
        decimal unitCost,
        int quantity,
        string unitOfMeasure,
        string? location,
        int reorderLevel)
    {
        Id = id;
        StockNumber = stockNumber;
        ProductId = productId;
        Description = description;
        UnitCost = unitCost;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        WeightedAverageCost = unitCost;
        Location = location;
        ReorderLevel = reorderLevel;

        QueueDomainEvent(new ConsumableInventoryCreated { ConsumableInventory = this });
    }

    public static ConsumableInventory Create(
        string stockNumber,
        Guid productId,
        string description,
        decimal unitCost,
        int quantity,
        string unitOfMeasure,
        string? location = null,
        int reorderLevel = 0)
    {
        ValidateConsumableItem(unitCost, quantity);
        return new ConsumableInventory(
            Guid.NewGuid(),
            stockNumber,
            productId,
            description,
            unitCost,
            quantity,
            unitOfMeasure,
            location,
            reorderLevel);
    }

    /// <summary>
    /// Receive consumable items (increase stock using weighted average method)
    /// </summary>
    public void ReceiveStock(int quantity, decimal unitCost, string? poNumber = null)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (unitCost <= 0) throw new ArgumentException("Unit cost must be greater than zero.");

        // Calculate new weighted average cost
        decimal totalCost = (WeightedAverageCost * Quantity) + (unitCost * quantity);
        int totalQuantity = Quantity + quantity;

        WeightedAverageCost = totalCost / totalQuantity;
        Quantity = totalQuantity;

        QueueDomainEvent(new ConsumableStockReceived
        {
            ConsumableInventory = this,
            QuantityReceived = quantity,
            UnitCost = unitCost,
            PONumber = poNumber
        });
    }

    /// <summary>
    /// Issue consumable items (decrease stock) - RSMI generated
    /// </summary>
    public void IssueStock(int quantity, Guid issuedTo, string? rsmiNumber = null)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (Quantity < quantity) throw new InvalidOperationException("Insufficient stock available.");

        Quantity -= quantity;

        QueueDomainEvent(new ConsumableStockIssued
        {
            ConsumableInventory = this,
            QuantityIssued = quantity,
            IssuedTo = issuedTo,
            RSMINumber = rsmiNumber,
            UnitCost = WeightedAverageCost
        });
    }

    /// <summary>
    /// Adjust stock (for physical count reconciliation)
    /// </summary>
    public void AdjustStock(int newQuantity, string reason)
    {
        if (newQuantity < 0) throw new ArgumentException("Quantity cannot be negative.");

        int adjustment = newQuantity - Quantity;
        Quantity = newQuantity;

        QueueDomainEvent(new ConsumableStockAdjusted
        {
            ConsumableInventory = this,
            Adjustment = adjustment,
            Reason = reason
        });
    }

    public ConsumableInventory UpdateDetails(
        string? description = null,
        string? location = null,
        int? reorderLevel = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description;
            isUpdated = true;
        }

        if (location != null && Location != location)
        {
            Location = location;
            isUpdated = true;
        }

        if (reorderLevel.HasValue && ReorderLevel != reorderLevel.Value)
        {
            ReorderLevel = reorderLevel.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ConsumableInventoryUpdated { ConsumableInventory = this });
        }

        return this;
    }

    private static void ValidateConsumableItem(decimal unitCost, int quantity)
    {
        if (unitCost <= 0) throw new ArgumentException("Unit cost must be greater than zero.");
        if (unitCost > 50000) throw new ArgumentException("Consumable items must be ≤ ₱50,000. Use PPE classification instead.");
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
    }
}

