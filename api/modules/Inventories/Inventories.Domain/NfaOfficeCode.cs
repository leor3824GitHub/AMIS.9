using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// NFA office codes based on COA Annex E.
/// </summary>
public sealed class NfaOfficeCode : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!; // e.g., 6000, 6001, 6800
    public string OfficeName { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? ParentOfficeCode { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;

    private NfaOfficeCode() { }

    private NfaOfficeCode(
        string code,
        string officeName,
        int sortOrder,
        string? description = null,
        string? parentOfficeCode = null)
    {
        Id = Guid.NewGuid();
        Code = code;
        OfficeName = officeName;
        Description = description;
        ParentOfficeCode = parentOfficeCode;
        SortOrder = sortOrder;
        IsActive = true;
    }

    public static NfaOfficeCode Create(
        string code,
        string officeName,
        int sortOrder = 10,
        string? description = null,
        string? parentOfficeCode = null)
    {
        Validate(code, officeName);

        return new NfaOfficeCode(code, officeName, sortOrder, description, parentOfficeCode);
    }

    public void Update(
        string officeName,
        int sortOrder,
        string? description = null,
        string? parentOfficeCode = null)
    {
        Validate(Code, officeName);

        OfficeName = officeName;
        SortOrder = sortOrder;
        Description = description;
        ParentOfficeCode = parentOfficeCode;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static void Validate(string code, string officeName)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Office code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(officeName))
            throw new ArgumentException("Office name is required.", nameof(officeName));
    }
}
