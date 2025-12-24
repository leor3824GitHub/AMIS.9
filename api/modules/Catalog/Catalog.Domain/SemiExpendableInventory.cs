using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Semi-Expendable Inventory - Durable items with useful life > 1 year (≤ ₱50,000)
/// RCA Account: 10599020 - Semi-Expendable Property Inventory
/// Expense Account: 50299010 - Semi-Expendable Property Expense
/// Required Document: ICS (Inventory Custodian Slip)
/// </summary>
public class SemiExpendableInventory : AuditableEntity, IAggregateRoot
{
    public string PropertyCode { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public string Description { get; private set; } = default!;
    public decimal UnitCost { get; private set; }
    public int Quantity { get; private set; }
    public string UnitOfMeasure { get; private set; } = default!;
    public decimal WeightedAverageCost { get; private set; }
    public string? Location { get; private set; }
    public int EstimatedUsefulLifeMonths { get; private set; }
    public Guid? CurrentCustodianId { get; private set; }
    public bool IsIssued { get; private set; }

    // Navigation
    public virtual Product Product { get; private set; } = default!;
    public virtual Employee? CurrentCustodian { get; private set; }

    private SemiExpendableInventory() { }

    private SemiExpendableInventory(
        Guid id,
        string propertyCode,
        Guid productId,
        string description,
        decimal unitCost,
        int quantity,
        string unitOfMeasure,
        string? location,
        int estimatedUsefulLifeMonths)
    {
        Id = id;
        PropertyCode = propertyCode;
        ProductId = productId;
        Description = description;
        UnitCost = unitCost;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        WeightedAverageCost = unitCost;
        Location = location;
        EstimatedUsefulLifeMonths = estimatedUsefulLifeMonths;
        IsIssued = false;

        QueueDomainEvent(new SemiExpendableInventoryCreated { SemiExpendableInventory = this });
    }

    public static SemiExpendableInventory Create(
        string propertyCode,
        Guid productId,
        string description,
        decimal unitCost,
        int quantity,
        string unitOfMeasure,
        string? location = null,
        int estimatedUsefulLifeMonths = 36)
    {
        ValidateSemiExpendableItem(unitCost, quantity);
        return new SemiExpendableInventory(
            Guid.NewGuid(),
            propertyCode,
            productId,
            description,
            unitCost,
            quantity,
            unitOfMeasure,
            location,
            estimatedUsefulLifeMonths);
    }

    /// <summary>
    /// Receive semi-expendable items (increase stock using weighted average method)
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

        QueueDomainEvent(new SemiExpendableStockReceived
        {
            SemiExpendableInventory = this,
            QuantityReceived = quantity,
            UnitCost = unitCost,
            PONumber = poNumber
        });
    }

    /// <summary>
    /// Issue semi-expendable items (decrease stock) - ICS generated
    /// Assigns item to custodian for accountability
    /// </summary>
    public void IssueStock(int quantity, Guid custodianId, string? icsNumber = null)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (Quantity < quantity) throw new InvalidOperationException("Insufficient stock available.");

        Quantity -= quantity;
        
        // If issuing all remaining stock to one person, mark as issued with custodian
        if (Quantity == 0)
        {
            CurrentCustodianId = custodianId;
            IsIssued = true;
        }

        QueueDomainEvent(new SemiExpendableStockIssued
        {
            SemiExpendableInventory = this,
            QuantityIssued = quantity,
            CustodianId = custodianId,
            ICSNumber = icsNumber,
            UnitCost = WeightedAverageCost
        });
    }

    /// <summary>
    /// Return semi-expendable item from custodian
    /// </summary>
    public void ReturnFromCustodian(int quantity, string? reason = null)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

        Quantity += quantity;
        
        // Clear custodian if returning
        if (IsIssued)
        {
            CurrentCustodianId = null;
            IsIssued = false;
        }

        QueueDomainEvent(new SemiExpendableStockReturned
        {
            SemiExpendableInventory = this,
            QuantityReturned = quantity,
            Reason = reason
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

        QueueDomainEvent(new SemiExpendableStockAdjusted
        {
            SemiExpendableInventory = this,
            Adjustment = adjustment,
            Reason = reason
        });
    }

    public SemiExpendableInventory UpdateDetails(
        string? description = null,
        string? location = null,
        int? estimatedUsefulLifeMonths = null)
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

        if (estimatedUsefulLifeMonths.HasValue && EstimatedUsefulLifeMonths != estimatedUsefulLifeMonths.Value)
        {
            EstimatedUsefulLifeMonths = estimatedUsefulLifeMonths.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new SemiExpendableInventoryUpdated { SemiExpendableInventory = this });
        }

        return this;
    }

    private static void ValidateSemiExpendableItem(decimal unitCost, int quantity)
    {
        if (unitCost <= 0) throw new ArgumentException("Unit cost must be greater than zero.");
        if (unitCost > 50000) throw new ArgumentException("Semi-expendable items must be ≤ ₱50,000. Use PPE classification instead.");
        if (unitCost < 1000) throw new ArgumentException("Items below ₱1,000 should be classified as consumables.");
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
    }
}
