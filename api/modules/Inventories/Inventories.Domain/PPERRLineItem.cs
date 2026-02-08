using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a line item in the PPE Receiving report
/// </summary>
public class PPERRLineItem : AuditableEntity
{
    public string PropertyCode { get; set; } = string.Empty;
    public string RRNumber { get; set; } = string.Empty;

    private PPERRLineItem()
    {
    }

    public PPERRLineItem(string propertyCode, string rrNumber)
    {
        if (string.IsNullOrWhiteSpace(propertyCode))
            throw new ArgumentException("Property code cannot be empty.", nameof(propertyCode));
        
        if (string.IsNullOrWhiteSpace(rrNumber))
            throw new ArgumentException("RR number cannot be empty.", nameof(rrNumber));

        PropertyCode = propertyCode;
        RRNumber = rrNumber;
    }
}


