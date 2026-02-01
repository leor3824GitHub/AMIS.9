namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a line item in the supplies and materials issuance report
/// </summary>
public record IssuanceLineItem
{
    public string PropertyCode { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime AcquisitionDate { get; }
    public decimal Quantity { get; }
    public string Unit { get; }
    public decimal UnitCost { get; }
    public decimal Amount => Quantity * UnitCost;

    public IssuanceLineItem(
        string propertyCode,
        string name,
        string description,
        DateTime acquisitionDate,
        decimal quantity,
        string unit,
        decimal unitCost)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Item description cannot be empty.", nameof(description));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty.", nameof(unit));

        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative.", nameof(unitCost));

        if (acquisitionDate > DateTime.UtcNow)
            throw new ArgumentException("Acquisition date cannot be in the future.", nameof(acquisitionDate));

        PropertyCode = propertyCode;
        Name = name;
        Description = description;
        AcquisitionDate = acquisitionDate;
        Quantity = quantity;
        Unit = unit;
        UnitCost = unitCost;
    }
}

