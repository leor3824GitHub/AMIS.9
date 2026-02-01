using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Tracks sequence numbers for COA-compliant property code generation.
/// </summary>
public sealed class PropertyCodeSequence : AuditableEntity, IAggregateRoot
{
    public string TenantId { get; private set; } = default!;
    public int YearKey { get; private set; } // 0 if not resetting annually
    public string OfficeCode { get; private set; } = default!;
    public string ClassCode { get; private set; } = default!;
    public string CategoryCode { get; private set; } = default!;
    public string ItemCode { get; private set; } = default!;
    public int LastSequence { get; private set; }
    public bool ResetAnnually { get; private set; }

    private PropertyCodeSequence() { }

    private PropertyCodeSequence(
        string tenantId,
        int yearKey,
        string officeCode,
        string classCode,
        string categoryCode,
        string itemCode,
        int lastSequence,
        bool resetAnnually)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        YearKey = yearKey;
        OfficeCode = officeCode;
        ClassCode = classCode;
        CategoryCode = categoryCode;
        ItemCode = itemCode;
        LastSequence = lastSequence;
        ResetAnnually = resetAnnually;
    }

    public static PropertyCodeSequence Create(
        string tenantId,
        int yearKey,
        string officeCode,
        string classCode,
        string categoryCode,
        string itemCode,
        bool resetAnnually)
    {
        Validate(tenantId, officeCode, classCode, categoryCode, itemCode);
        return new PropertyCodeSequence(tenantId, yearKey, officeCode, classCode, categoryCode, itemCode, 0, resetAnnually);
    }

    public int Increment()
    {
        LastSequence++;
        return LastSequence;
    }

    private static void Validate(string tenantId, string officeCode, string classCode, string categoryCode, string itemCode)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("TenantId is required.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(officeCode))
            throw new ArgumentException("Office code is required.", nameof(officeCode));
        if (string.IsNullOrWhiteSpace(classCode))
            throw new ArgumentException("Class code is required.", nameof(classCode));
        if (string.IsNullOrWhiteSpace(categoryCode))
            throw new ArgumentException("Category code is required.", nameof(categoryCode));
        if (string.IsNullOrWhiteSpace(itemCode))
            throw new ArgumentException("Item code is required.", nameof(itemCode));
    }
}
