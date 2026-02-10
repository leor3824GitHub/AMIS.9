using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Tracks sequence numbers for property code generation.
/// Aligns with COA/DBM property classification codes (NFA standards).
/// Format: {Year}-{OfficeCode}-{ClassCode}-{CategoryCode}-{ItemCode}-{Sequence:D4}.0
/// Example: 2026-NFA-OE-01-001-0001.0
/// </summary>
public sealed class PropertyCodeSequence : AuditableEntity, IAggregateRoot
{
    public int Id { get; set; }
    
    /// <summary>
    /// Short classification code (e.g., OE, DP, LT, FF, TS).
    /// Represents the major asset category.
    /// </summary>
    public string ClassCode { get; set; } = default!;
    
    /// <summary>
    /// Full classification description (e.g., OFFICE EQUIPMENT, INFORMATION & COMMUNICATION TECHNOLOGY EQPT.).
    /// </summary>
    public string Classification { get; set; } = default!;
    
    /// <summary>
    /// Category code for sub-classification (e.g., 01, 02, 03).
    /// Represents specific asset types within ClassCode.
    /// </summary>
    public string CategoryCode { get; set; } = default!;
    
    /// <summary>
    /// Item code for specific item type within category (e.g., 001, 002, 003).
    /// Represents specific PPE items like ADDING MACHINE, CALCULATORS, PERSONAL COMPUTERS.
    /// </summary>
    public string ItemCode { get; set; } = default!;
    
    /// <summary>
    /// Item description detailing the specific asset type (e.g., ADDING MACHINE, PERSONAL COMPUTERS).
    /// Provides human-readable name for ItemCode.
    /// </summary>
    public string ItemDescription { get; set; } = default!;
    
    /// <summary>
    /// GL Account code for accounting integration (e.g., 10605020, 10605030).
    /// Maps to Chart of Accounts for financial reporting.
    /// </summary>
    public string? GLAccount { get; set; }
    
    /// <summary>
    /// Last allocated sequence number for this ClassCode/CategoryCode/ItemCode combination.
    /// Incremented on each property code allocation.
    /// </summary>
    public int LastSequenceValue { get; set; }

    private PropertyCodeSequence() { }

    private PropertyCodeSequence(string classCode, string classification, string categoryCode, string itemCode, string itemDescription, string? glAccount, int lastSequenceValue)
    {
        ClassCode = classCode;
        Classification = classification;
        CategoryCode = categoryCode;
        ItemCode = itemCode;
        ItemDescription = itemDescription;
        GLAccount = glAccount;
        LastSequenceValue = lastSequenceValue;
    }

    /// <summary>
    /// Factory method for backward compatibility with existing handlers using Classification/Category.
    /// Maps Classification to ClassCode and Category to CategoryCode.
    /// </summary>
    public static PropertyCodeSequence Create(string classification, string category)
    {
        if (string.IsNullOrWhiteSpace(classification))
            throw new ArgumentException("Classification is required.", nameof(classification));
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required.", nameof(category));

        return new PropertyCodeSequence(classification, classification, category, "001", "Standard Item", null, 0);
    }

    /// <summary>
    /// Factory method with full COA/DBM standard properties including item details.
    /// </summary>
    public static PropertyCodeSequence Create(string classCode, string classification, string categoryCode, string itemCode, string itemDescription, string? glAccount = null)
    {
        if (string.IsNullOrWhiteSpace(classCode))
            throw new ArgumentException("ClassCode is required.", nameof(classCode));
        if (string.IsNullOrWhiteSpace(classification))
            throw new ArgumentException("Classification is required.", nameof(classification));
        if (string.IsNullOrWhiteSpace(categoryCode))
            throw new ArgumentException("CategoryCode is required.", nameof(categoryCode));
        if (string.IsNullOrWhiteSpace(itemCode))
            throw new ArgumentException("ItemCode is required.", nameof(itemCode));
        if (string.IsNullOrWhiteSpace(itemDescription))
            throw new ArgumentException("ItemDescription is required.", nameof(itemDescription));

        return new PropertyCodeSequence(classCode, classification, categoryCode, itemCode, itemDescription, glAccount, 0);
    }

    public int Increment()
    {
        LastSequenceValue++;
        return LastSequenceValue;
    }
}
