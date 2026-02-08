using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a line item in the PPE Issuance report
/// </summary>
public class PPEIRLineItem : AuditableEntity
{
    public string PropertyCode { get; set; } = string.Empty;
    public string IRNumber { get; set; } = string.Empty;

    private PPEIRLineItem()
    {
    }

    public PPEIRLineItem(string propertyCode, string irNumber)
    {
        if (string.IsNullOrWhiteSpace(propertyCode))
            throw new ArgumentException("Property code cannot be empty.", nameof(propertyCode));
        
        if (string.IsNullOrWhiteSpace(irNumber))
            throw new ArgumentException("IR number cannot be empty.", nameof(irNumber));

        PropertyCode = propertyCode;
        IRNumber = irNumber;
    }
}
