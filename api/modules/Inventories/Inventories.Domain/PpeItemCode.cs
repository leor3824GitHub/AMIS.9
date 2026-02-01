using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// COA PPE item codes (3-digit) under a class and category.
/// </summary>
public sealed class PpeItemCode : AuditableEntity, IAggregateRoot
{
    public string ClassCode { get; private set; } = default!; // e.g., OE, DP
    public string CategoryCode { get; private set; } = default!; // e.g., 01, 02
    public string Code { get; private set; } = default!; // 3-digit item code
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? COAReference { get; private set; }

    private PpeItemCode() { }

    private PpeItemCode(
        string classCode,
        string categoryCode,
        string code,
        string name,
        int sortOrder,
        string? description = null,
        string? coaReference = null)
    {
        Id = Guid.NewGuid();
        ClassCode = classCode;
        CategoryCode = categoryCode;
        Code = code;
        Name = name;
        Description = description;
        SortOrder = sortOrder;
        COAReference = coaReference;
        IsActive = true;
    }

    public static PpeItemCode Create(
        string classCode,
        string categoryCode,
        string code,
        string name,
        int sortOrder = 10,
        string? description = null,
        string? coaReference = null)
    {
        Validate(classCode, categoryCode, code, name);

        return new PpeItemCode(classCode, categoryCode, code, name, sortOrder, description, coaReference);
    }

    public void Update(
        string name,
        int sortOrder,
        string? description = null,
        string? coaReference = null)
    {
        Validate(ClassCode, CategoryCode, Code, name);

        Name = name;
        SortOrder = sortOrder;
        Description = description;
        COAReference = coaReference;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static void Validate(string classCode, string categoryCode, string code, string name)
    {
        if (string.IsNullOrWhiteSpace(classCode))
            throw new ArgumentException("Class code is required.", nameof(classCode));
        if (string.IsNullOrWhiteSpace(categoryCode))
            throw new ArgumentException("Category code is required.", nameof(categoryCode));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Item code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name is required.", nameof(name));
    }
}
