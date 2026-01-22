namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a line item in the supplies and materials receiving report
/// </summary>
public record ReceivingLineItem
{
    public string Name { get; }
    public string Description { get; }
    public string? Reference { get; } // Link to PO or delivery receipt
    public DateTime AcquisitionDate { get; }
    public decimal Quantity { get; }
    public string Unit { get; }
    public decimal UnitCost { get; }
    public string Location { get; }
    public decimal Amount => Quantity * UnitCost;

    public ReceivingLineItem(
        string name,
        string description,
        DateTime acquisitionDate,
        decimal quantity,
        string unit,
        decimal unitCost,
        string location,
        string? reference = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Item description cannot be empty.", nameof(description));

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Item location cannot be empty.", nameof(location));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty.", nameof(unit));

        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative.", nameof(unitCost));

        if (acquisitionDate > DateTime.UtcNow)
            throw new ArgumentException("Acquisition date cannot be in the future.", nameof(acquisitionDate));

        Name = name;
        Description = description;
        Reference = reference;
        AcquisitionDate = acquisitionDate;
        Quantity = quantity;
        Unit = unit;
        UnitCost = unitCost;
        Location = location;
    }
}

