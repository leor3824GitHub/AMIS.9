using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// COA PPE category codes (e.g., LL, BS, OS) with account code mappings.
/// </summary>
public sealed class PpeCategoryCode : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!; // e.g., LL, BS, OS
    public string AccountCode { get; private set; } = default!; // e.g., 10601010
    public string Name { get; private set; } = default!; // e.g., LAND
    public string? Description { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? COAReference { get; private set; }

    public ICollection<PpeTypeCode> TypeCodes { get; private set; } = new List<PpeTypeCode>();

    private PpeCategoryCode() { }

    private PpeCategoryCode(
        string code,
        string accountCode,
        string name,
        int sortOrder,
        string? description = null,
        string? coaReference = null)
    {
        Id = Guid.NewGuid();
        Code = code;
        AccountCode = accountCode;
        Name = name;
        Description = description;
        SortOrder = sortOrder;
        COAReference = coaReference;
        IsActive = true;
    }

    public static PpeCategoryCode Create(
        string code,
        string accountCode,
        string name,
        int sortOrder = 10,
        string? description = null,
        string? coaReference = null)
    {
        Validate(code, accountCode, name);

        return new PpeCategoryCode(code, accountCode, name, sortOrder, description, coaReference);
    }

    public void Update(
        string accountCode,
        string name,
        int sortOrder,
        string? description = null,
        string? coaReference = null)
    {
        Validate(Code, accountCode, name);

        AccountCode = accountCode;
        Name = name;
        SortOrder = sortOrder;
        Description = description;
        COAReference = coaReference;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static void Validate(string code, string accountCode, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("PPE category code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(accountCode))
            throw new ArgumentException("Account code is required.", nameof(accountCode));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("PPE category name is required.", nameof(name));
    }
}
