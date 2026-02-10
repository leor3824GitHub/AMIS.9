using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain.Services;

/// <summary>
/// Database-driven implementation of IAssetClassificationPolicy.
/// Loads asset classification rules from database and uses them to determine classification.
/// 
/// Features:
/// - Supports multiple overlapping rules with cost ranges
/// - Effective date and expiry date validation for rule applicability
/// - Priority-based rule ranking for conflict resolution
/// - Fallback to default threshold policy if no rule matches
/// - Type-safe cost range validation
/// 
/// Rule Resolution:
/// 1. Filter active rules within their effective date range
/// 2. Order by priority (lower number = higher priority)
/// 3. Match acquisition cost to rule's MinimumCost..MaximumCost range
/// 4. Return first matching rule's classification
/// 5. Fallback to DefaultAssetClassificationPolicy if no match
/// </summary>
public class DatabaseAssetClassificationPolicy : IAssetClassificationPolicy
{
    private readonly IReadOnlyCollection<AssetClassificationRule> _rules;
    private readonly DateTime _evaluationDate;
    private readonly IAssetClassificationPolicy _fallbackPolicy;

    /// <summary>
    /// Creates a database-driven policy from a collection of asset classification rules.
    /// </summary>
    /// <param name="rules">Collection of asset classification rules to load from database.</param>
    /// <param name="evaluationDate">The date to use for effective/expiry date checks. If null, uses current UTC time.</param>
    /// <exception cref="ArgumentNullException">Thrown when rules collection is null.</exception>
    public DatabaseAssetClassificationPolicy(
        IEnumerable<AssetClassificationRule> rules,
        DateTime? evaluationDate = null)
    {
        if (rules == null)
            throw new ArgumentNullException(nameof(rules), "Rules collection cannot be null.");

        _evaluationDate = evaluationDate ?? DateTime.UtcNow;
        _fallbackPolicy = new DefaultAssetClassificationPolicy();

        // Filter active rules within date range and order by priority
        _rules = rules
            .Where(IsRuleActive)
            .OrderBy(r => r.Priority)
            .ThenBy(r => r.MinimumCost)  // Stable secondary sorting by cost
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Determines asset classification by matching acquisition cost to rule cost ranges.
    /// Priority ordering ensures consistent behavior with overlapping rules.
    /// </summary>
    /// <param name="acquisitionCost">The acquisition cost of the asset in pesos.</param>
    /// <returns>
    /// Classification from highest-priority matching rule, or fallback default if no rule matches.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when acquisition cost is negative.</exception>
    public PropertyClassification DetermineClassification(decimal acquisitionCost)
    {
        ValidateCost(acquisitionCost);

        // Find first matching rule based on cost range
        var matchingRule = _rules.FirstOrDefault(r =>
            acquisitionCost >= r.MinimumCost &&
            acquisitionCost <= r.MaximumCost);

        if (matchingRule != null)
        {
            return matchingRule.Classification;
        }

        // No matching rule found - use fallback to default thresholds
        return _fallbackPolicy.DetermineClassification(acquisitionCost);
    }

    /// <summary>
    /// Gets the RCA account code for an asset classification.
    /// Uses database rule mapping or falls back to standard DBM codes.
    /// </summary>
    /// <param name="classification">The asset classification.</param>
    /// <returns>RCA account code as string.</returns>
    /// <exception cref="InvalidOperationException">Thrown for unknown classifications.</exception>
    public string GetRCAAccountCode(PropertyClassification classification)
    {
        // Find highest-priority rule for this classification
        var rule = _rules.FirstOrDefault(r => r.Classification == classification);

        if (rule != null && !string.IsNullOrWhiteSpace(rule.RCAAccountCode))
        {
            return rule.RCAAccountCode;
        }

        // Fallback to standard mappings
        return _fallbackPolicy.GetRCAAccountCode(classification);
    }

    /// <summary>
    /// Gets the number of active rules currently in use.
    /// Useful for diagnostics and monitoring rule-set coverage.
    /// </summary>
    public int ActiveRuleCount => _rules.Count;

    /// <summary>
    /// Gets diagnostic information about rule coverage for a cost range.
    /// Useful for testing and configuration validation.
    /// </summary>
    /// <param name="minCost">Minimum cost to check.</param>
    /// <param name="maxCost">Maximum cost to check.</param>
    /// <returns>Collection of rules covering the cost range.</returns>
    public IEnumerable<AssetClassificationRule> GetRulesCoveringRange(decimal minCost, decimal maxCost)
    {
        return _rules.Where(r => 
            !(r.MaximumCost < minCost || r.MinimumCost > maxCost));
    }

    /// <summary>
    /// Validates that a rule has valid cost boundaries (minimum &lt;= maximum).
    /// </summary>
    /// <param name="rule">The rule to validate.</param>
    /// <returns>True if valid; false otherwise.</returns>
    public static bool IsRuleCostValid(AssetClassificationRule rule)
    {
        return rule != null && rule.MinimumCost <= rule.MaximumCost;
    }

    /// <summary>
    /// Checks if a rule is currently active based on its status and date range.
    /// </summary>
    private bool IsRuleActive(AssetClassificationRule rule)
    {
        if (rule == null || !rule.IsActive)
            return false;

        // Check effective date (rule must be on or after effective date)
        if (_evaluationDate < rule.EffectiveDate)
            return false;

        // Check expiry date (rule must be before or null expiry date)
        if (rule.ExpiryDate.HasValue && _evaluationDate > rule.ExpiryDate)
            return false;

        // Validate cost range
        if (!IsRuleCostValid(rule))
            return false;

        return true;
    }

    /// <summary>
    /// Validates acquisition cost is non-negative.
    /// </summary>
    private static void ValidateCost(decimal cost)
    {
        if (cost < 0)
            throw new ArgumentException("Acquisition cost cannot be negative.", nameof(cost));
    }
}

/// <summary>
/// Default fallback implementation of IAssetClassificationPolicy.
/// Uses fixed DBM-standard thresholds when no database rules match.
/// 
/// Standard Classification Thresholds:
/// - Property Plant &amp; Equipment (PPE): Acquisition Cost ≥ ₱50,000
/// - Semi-Expendable: ₱500 ≤ Acquisition Cost &lt; ₱50,000
/// - Consumable: Acquisition Cost &lt; ₱500
/// 
/// These thresholds follow Department of Budget &amp; Management (DBM) guidelines
/// and serve as system defaults when custom database rules aren't configured.
/// As per COA guidelines, these can be updated by authorized policies.
/// </summary>
public class DefaultAssetClassificationPolicy : IAssetClassificationPolicy
{
    /// <summary>
    /// Threshold for Property Plant &amp; Equipment classification.
    /// Assets with acquisition cost ≥ ₱50,000 are classified as PPE.
    /// Per DBM standard thresholds.
    /// </summary>
    private const decimal PpeThreshold = 50000m;

    /// <summary>
    /// Threshold for Semi-Expendable classification.
    /// Assets with ₱500 ≤ cost &lt; ₱50,000 are classified as semi-expendable.
    /// Per DBM standard thresholds.
    /// </summary>
    private const decimal SemiExpendableThreshold = 500m;

    /// <summary>
    /// Determines asset classification using DBM-standard fixed thresholds.
    /// Maximum precision - does not require database access.
    /// </summary>
    /// <param name="acquisitionCost">The acquisition cost of the asset in pesos.</param>
    /// <returns>
    /// Classification per DBM guidelines:
    /// - PropertyPlantEquipment if cost ≥ ₱50,000
    /// - SemiExpendable if ₱500 ≤ cost &lt; ₱50,000
    /// - Consumable if cost &lt; ₱500
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when cost is negative.</exception>
    public PropertyClassification DetermineClassification(decimal acquisitionCost)
    {
        if (acquisitionCost < 0)
            throw new ArgumentException("Acquisition cost cannot be negative.", nameof(acquisitionCost));

        return acquisitionCost switch
        {
            >= PpeThreshold => PropertyClassification.PropertyPlantEquipment,
            >= SemiExpendableThreshold => PropertyClassification.SemiExpendable,
            _ => PropertyClassification.Consumable
        };
    }

    /// <summary>
    /// Gets the RCA account code for a classification using standard DBM mappings.
    /// </summary>
    /// <param name="classification">The asset classification.</param>
    /// <returns>
    /// RCA account code:
    /// - Consumable → Supplies and Materials Inventory
    /// - SemiExpendable → Semi-Expendable Property Inventory
    /// - PropertyPlantEquipment → Other Property, Plant and Equipment
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown for unknown classifications.</exception>
    public string GetRCAAccountCode(PropertyClassification classification)
    {
        return classification switch
        {
            PropertyClassification.Consumable => ValueObjects.RCAAccountCode.SuppliesAndMaterialsInventory,
            PropertyClassification.SemiExpendable => ValueObjects.RCAAccountCode.SemiExpendablePropertyInventory,
            PropertyClassification.PropertyPlantEquipment => ValueObjects.RCAAccountCode.OtherPropertyPlantAndEquipment,
            _ => throw new InvalidOperationException($"Unknown classification: {classification}")
        };
    }
}

/// <summary>
/// Factory for creating and managing IAssetClassificationPolicy instances.
/// Provides convenient methods including caching and composite strategies.
/// </summary>
public static class AssetClassificationPolicyFactory
{
    /// <summary>
    /// Singleton instance of the default fallback policy (cached for efficiency).
    /// </summary>
    private static IAssetClassificationPolicy? _cachedDefaultPolicy;

    /// <summary>
    /// Creates a database-driven policy from asset classification rules.
    /// Rules are validated and filtered for active status and date range.
    /// </summary>
    /// <param name="rules">Collection of asset classification rules from database.</param>
    /// <param name="evaluationDate">Optional date for evaluating rule effective periods. Defaults to current UTC now.</param>
    /// <returns>A new DatabaseAssetClassificationPolicy instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if rules collection is null.</exception>
    public static IAssetClassificationPolicy CreateFromRules(
        IEnumerable<AssetClassificationRule> rules,
        DateTime? evaluationDate = null)
    {
        return new DatabaseAssetClassificationPolicy(rules, evaluationDate);
    }

    /// <summary>
    /// Gets the default fallback policy as a cached singleton.
    /// Uses fixed DBM-standard thresholds for maximum performance.
    /// </summary>
    /// <returns>Cached DefaultAssetClassificationPolicy instance.</returns>
    public static IAssetClassificationPolicy GetDefault()
    {
        return _cachedDefaultPolicy ??= new DefaultAssetClassificationPolicy();
    }

    /// <summary>
    /// Creates a policy that combines database rules with automatic fallback to defaults.
    /// This is the recommended approach for production use.
    /// </summary>
    /// <param name="rules">Collection of asset classification rules from database.</param>
    /// <param name="evaluationDate">Optional date for rule effective period validation.</param>
    /// <returns>
    /// A DatabaseAssetClassificationPolicy that internally falls back to default thresholds
    /// for any costs not covered by configured rules.
    /// </returns>
    public static IAssetClassificationPolicy CreateHybrid(
        IEnumerable<AssetClassificationRule> rules,
        DateTime? evaluationDate = null)
    {
        // DatabaseAssetClassificationPolicy internally implements this hybrid behavior
        return CreateFromRules(rules, evaluationDate);
    }
}
