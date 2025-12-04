using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;

public class RcaAccountCodeDefinition : AuditableEntity, IAggregateRoot
{
    public string Key { get; private set; } = null!;
    public string AccountCode { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private RcaAccountCodeDefinition() { }

    private RcaAccountCodeDefinition(string key, string accountCode, string? description, bool isActive)
    {
        Key = key;
        AccountCode = accountCode;
        Description = description;
        IsActive = isActive;
    }

    public static RcaAccountCodeDefinition Create(string key, string accountCode, string? description = null, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key is required", nameof(key));
        }

        if (string.IsNullOrWhiteSpace(accountCode))
        {
            throw new ArgumentException("Account code is required", nameof(accountCode));
        }

        return new RcaAccountCodeDefinition(key, accountCode, description, isActive);
    }

    public void UpdateAccountCode(string accountCode, string? description)
    {
        if (string.IsNullOrWhiteSpace(accountCode))
        {
            throw new ArgumentException("Account code is required", nameof(accountCode));
        }

        AccountCode = accountCode;
        Description = description;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
