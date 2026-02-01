using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// COA PPE type codes under a PPE category code (e.g., 01, 02, 03).
/// </summary>
public sealed class PpeTypeCode : AuditableEntity, IAggregateRoot
{
    public Guid CategoryId { get; private set; }
    public PpeCategoryCode Category { get; private set; } = default!;

    public string Code { get; private set; } = default!; // e.g., 01, 02
    public string Name { get; private set; } = default!; // e.g., LAND
    public string? Description { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? COAReference { get; private set; }

    private PpeTypeCode() { }

    private PpeTypeCode(
        Guid categoryId,
        string code,
        string name,
        int sortOrder,
        string? description = null,
        string? coaReference = null)
    {
        Id = Guid.NewGuid();
        CategoryId = categoryId;
        Code = code;
        Name = name;
        Description = description;
        SortOrder = sortOrder;
        COAReference = coaReference;
        IsActive = true;
    }

    public static PpeTypeCode Create(
        Guid categoryId,
        string code,
        string name,
        int sortOrder = 10,
        string? description = null,
        string? coaReference = null)
    {
        Validate(categoryId, code, name);

        return new PpeTypeCode(categoryId, code, name, sortOrder, description, coaReference);
    }

    public void Update(
        string name,
        int sortOrder,
        string? description = null,
        string? coaReference = null)
    {
        Validate(CategoryId, Code, name);

        Name = name;
        SortOrder = sortOrder;
        Description = description;
        COAReference = coaReference;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static void Validate(Guid categoryId, string code, string name)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category is required.", nameof(categoryId));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("PPE type code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("PPE type name is required.", nameof(name));
    }
}
