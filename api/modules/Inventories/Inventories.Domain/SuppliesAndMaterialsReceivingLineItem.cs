using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a line item in the Supplies and Materials Receiving report
/// </summary>
public class SuppliesAndMaterialsReceivingLineItem : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public decimal UnitCost { get; set; }
    public string? Location { get; set; }
    public string? Reference { get; set; }
    public string? ClassCode { get; set; }
    public string? CategoryCode { get; set; }
    public string? ItemCode { get; set; }

    public decimal Amount => Quantity * UnitCost;

    private SuppliesAndMaterialsReceivingLineItem()
    {
    }

    public SuppliesAndMaterialsReceivingLineItem(
        string name,
        string? description = null,
        DateTime? acquisitionDate = null,
        decimal quantity = 0,
        string? unit = null,
        decimal unitCost = 0,
        string? location = null,
        string? reference = null,
        string? classCode = null,
        string? categoryCode = null,
        string? itemCode = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty.", nameof(name));

        Name = name;
        Description = description;
        AcquisitionDate = acquisitionDate ?? DateTime.UtcNow;
        Quantity = quantity;
        Unit = unit;
        UnitCost = unitCost;
        Location = location;
        Reference = reference;
        ClassCode = classCode;
        CategoryCode = categoryCode;
        ItemCode = itemCode;
    }
}
