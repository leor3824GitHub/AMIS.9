using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Configurable mapping between PPE types and RCA account codes
/// Allows administrators to manage mappings without code changes
/// </summary>
public class PPETypeAccountMapping : AuditableEntity, IAggregateRoot
{
    public string PPEType { get; private set; } = default!;
    public string RCAAccountCode { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    private PPETypeAccountMapping() { }

    private PPETypeAccountMapping(
        string ppeType,
        string rcaAccountCode,
        string description)
    {
        Id = Guid.NewGuid();
        PPEType = ppeType;
        RCAAccountCode = rcaAccountCode;
        Description = description;
    }

    public static PPETypeAccountMapping Create(
        string ppeType,
        string rcaAccountCode,
        string description)
    {
        if (string.IsNullOrWhiteSpace(ppeType))
            throw new ArgumentException("PPE type is required.", nameof(ppeType));
        if (string.IsNullOrWhiteSpace(rcaAccountCode))
            throw new ArgumentException("RCA account code is required.", nameof(rcaAccountCode));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        return new PPETypeAccountMapping(ppeType, rcaAccountCode, description);
    }

    public void Update(string rcaAccountCode, string description)
    {
        if (string.IsNullOrWhiteSpace(rcaAccountCode))
            throw new ArgumentException("RCA account code is required.", nameof(rcaAccountCode));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        RCAAccountCode = rcaAccountCode;
        Description = description;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
