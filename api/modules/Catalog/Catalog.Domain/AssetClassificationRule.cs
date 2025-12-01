using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Configurable asset classification rules based on COA/DBM circulars
/// Allows threshold changes without code modification
/// </summary>
public class AssetClassificationRule : AuditableEntity, IAggregateRoot
{
    public string RuleName { get; private set; } = default!;
    public PropertyClassification Classification { get; private set; }
    public decimal MinimumCost { get; private set; }
    public decimal MaximumCost { get; private set; }
    public int? MinimumUsefulLifeMonths { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public string RCAAccountCode { get; private set; } = default!;
    public string ExpenseAccountCode { get; private set; } = default!;
    public string DocumentType { get; private set; } = default!; // RSMI, ICS, PAR
    public string COAReference { get; private set; } = default!; // e.g., "COA Circular 2022-004"
    public bool IsActive { get; private set; } = true;
    public int Priority { get; private set; } // For overlapping rules

    private AssetClassificationRule() { }

    public static AssetClassificationRule Create(
        string ruleName,
        PropertyClassification classification,
        decimal minimumCost,
        decimal maximumCost,
        DateTime effectiveDate,
        string rcaAccountCode,
        string expenseAccountCode,
        string documentType,
        string coaReference,
        int? minimumUsefulLifeMonths = null,
        int priority = 10)
    {
        return new AssetClassificationRule
        {
            Id = Guid.NewGuid(),
            RuleName = ruleName,
            Classification = classification,
            MinimumCost = minimumCost,
            MaximumCost = maximumCost,
            MinimumUsefulLifeMonths = minimumUsefulLifeMonths,
            EffectiveDate = effectiveDate,
            RCAAccountCode = rcaAccountCode,
            ExpenseAccountCode = expenseAccountCode,
            DocumentType = documentType,
            COAReference = coaReference,
            Priority = priority,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetExpiryDate(DateTime expiryDate)
    {
        if (expiryDate < EffectiveDate)
            throw new ArgumentException("Expiry date cannot be before effective date.");

        ExpiryDate = expiryDate;
    }

    public bool IsEffectiveOn(DateTime date)
    {
        if (!IsActive)
            return false;

        if (date < EffectiveDate)
            return false;

        if (ExpiryDate.HasValue && date > ExpiryDate.Value)
            return false;

        return true;
    }

    public bool AppliesToAsset(decimal acquisitionCost, int? usefulLifeMonths = null)
    {
        if (!IsActive)
            return false;

        if (acquisitionCost < MinimumCost || acquisitionCost > MaximumCost)
            return false;

        if (MinimumUsefulLifeMonths.HasValue && usefulLifeMonths.HasValue)
        {
            if (usefulLifeMonths.Value < MinimumUsefulLifeMonths.Value)
                return false;
        }

        return true;
    }
}