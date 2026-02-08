using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Minimal asset classification rule for cost-based categorization.
/// Used by DatabaseAssetClassificationPolicy to determine asset classification.
/// </summary>
public class AssetClassificationRule : AuditableEntity, IAggregateRoot
{
    /// <summary>Name of the rule for reference (e.g., "PPE 2026").</summary>
    public string Name { get; private set; } = default!;

    /// <summary>Classification assigned to assets within this cost range.</summary>
    public PropertyClassification Classification { get; private set; }

    /// <summary>Minimum acquisition cost for this classification (inclusive).</summary>
    public decimal MinimumCost { get; private set; }

    /// <summary>Maximum acquisition cost for this classification (inclusive).</summary>
    public decimal MaximumCost { get; private set; }

    /// <summary>Date when this rule becomes effective.</summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>Optional date when this rule expires. Null means no expiration.</summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>RCA account code for this classification (e.g., "1030").</summary>
    public string RCAAccountCode { get; private set; } = default!;

    /// <summary>Priority for conflict resolution when cost ranges overlap (0 = highest).</summary>
    public int Priority { get; private set; }

    /// <summary>Whether this rule is currently in effect.</summary>
    public bool IsActive { get; private set; }

    private AssetClassificationRule() { }

    /// <summary>Creates a new asset classification rule.</summary>
    public static AssetClassificationRule Create(
        string name,
        PropertyClassification classification,
        decimal minimumCost,
        decimal maximumCost,
        DateTime effectiveDate,
        string rcaAccountCode,
        int priority = 10,
        DateTime? expiryDate = null)
    {
        if (minimumCost < 0 || maximumCost < 0)
            throw new ArgumentException("Costs cannot be negative.");

        if (minimumCost > maximumCost)
            throw new ArgumentException("MinimumCost cannot exceed MaximumCost.");

        if (expiryDate.HasValue && expiryDate < effectiveDate)
            throw new ArgumentException("ExpiryDate cannot be before EffectiveDate.");

        return new AssetClassificationRule
        {
            Id = Guid.NewGuid(),
            Name = name,
            Classification = classification,
            MinimumCost = minimumCost,
            MaximumCost = maximumCost,
            EffectiveDate = effectiveDate,
            ExpiryDate = expiryDate,
            RCAAccountCode = rcaAccountCode,
            Priority = priority,
            IsActive = true
        };
    }

    /// <summary>Deactivates this rule without deleting it.</summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>Activates a previously deactivated rule.</summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>Updates the name of this rule.</summary>
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");
        Name = name;
    }

    /// <summary>Updates the minimum cost threshold.</summary>
    public void SetMinimumCost(decimal minimumCost)
    {
        if (minimumCost < 0)
            throw new ArgumentException("Minimum cost cannot be negative.");
        if (minimumCost > MaximumCost)
            throw new ArgumentException("MinimumCost cannot exceed MaximumCost.");
        MinimumCost = minimumCost;
    }

    /// <summary>Updates the maximum cost threshold.</summary>
    public void SetMaximumCost(decimal maximumCost)
    {
        if (maximumCost < 0)
            throw new ArgumentException("Maximum cost cannot be negative.");
        if (maximumCost < MinimumCost)
            throw new ArgumentException("MaximumCost cannot be less than MinimumCost.");
        MaximumCost = maximumCost;
    }

    /// <summary>Updates the effective date of this rule.</summary>
    public void SetEffectiveDate(DateTime effectiveDate)
    {
        if (ExpiryDate.HasValue && effectiveDate > ExpiryDate.Value)
            throw new ArgumentException("EffectiveDate cannot be after ExpiryDate.");
        EffectiveDate = effectiveDate;
    }

    /// <summary>Updates the expiry date of this rule.</summary>
    public void SetExpiryDate(DateTime? expiryDate)
    {
        if (expiryDate.HasValue && expiryDate.Value < EffectiveDate)
            throw new ArgumentException("ExpiryDate cannot be before EffectiveDate.");
        ExpiryDate = expiryDate;
    }

    /// <summary>Updates the RCA account code.</summary>
    public void SetRCAAccountCode(string rcaAccountCode)
    {
        if (string.IsNullOrWhiteSpace(rcaAccountCode))
            throw new ArgumentException("RCA Account Code cannot be empty.");
        RCAAccountCode = rcaAccountCode;
    }

    /// <summary>Updates the priority of this rule.</summary>
    public void SetPriority(int priority)
    {
        if (priority <= 0)
            throw new ArgumentException("Priority must be greater than 0.");
        Priority = priority;
    }
}
