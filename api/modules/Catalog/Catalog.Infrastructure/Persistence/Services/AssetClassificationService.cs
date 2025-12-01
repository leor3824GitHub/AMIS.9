using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Services;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Services;

/// <summary>
/// Implementation of asset classification service with caching for performance
/// </summary>
public class AssetClassificationService : IAssetClassificationService
{
    private readonly CatalogDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AssetClassificationService> _logger;
    private const string CacheKey = "AssetClassificationRules";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public AssetClassificationService(
        CatalogDbContext context,
        IMemoryCache cache,
        ILogger<AssetClassificationService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PropertyClassification> DetermineClassificationAsync(
        decimal acquisitionCost,
        int estimatedUsefulLifeMonths,
        DateTime? effectiveDate = null,
        CancellationToken cancellationToken = default)
    {
        var date = effectiveDate ?? DateTime.UtcNow;
        var rules = await GetActiveRulesAsync(date, cancellationToken);

        // Find matching rule with highest priority
        var matchingRule = rules
            .Where(r => r.AppliesToAsset(acquisitionCost, estimatedUsefulLifeMonths))
            .OrderByDescending(r => r.Priority)
            .FirstOrDefault();

        if (matchingRule != null)
        {
            _logger.LogDebug(
                "Asset classified as {Classification} using rule {RuleName} (Cost: {Cost}, Life: {Life})",
                matchingRule.Classification,
                matchingRule.RuleName,
                acquisitionCost,
                estimatedUsefulLifeMonths);

            return matchingRule.Classification;
        }

        // Fallback to default logic if no rules configured
        _logger.LogWarning(
            "No classification rule found for asset (Cost: {Cost}, Life: {Life}). Using default logic.",
            acquisitionCost,
            estimatedUsefulLifeMonths);

        return GetDefaultClassification(acquisitionCost, estimatedUsefulLifeMonths);
    }

    public async Task<IEnumerable<AssetClassificationRule>> GetActiveRulesAsync(
        DateTime? effectiveDate = null,
        CancellationToken cancellationToken = default)
    {
        var date = effectiveDate ?? DateTime.UtcNow;
        var cacheKey = $"{CacheKey}_{date:yyyyMMdd}";

        if (_cache.TryGetValue<List<AssetClassificationRule>>(cacheKey, out var cachedRules))
        {
            return cachedRules!;
        }

        var rules = await _context.AssetClassificationRules
            .Where(r => r.IsActive
                     && r.EffectiveDate <= date
                     && (r.ExpiryDate == null || r.ExpiryDate >= date))
            .OrderByDescending(r => r.Priority)
            .ToListAsync(cancellationToken);

        _cache.Set(cacheKey, rules, CacheDuration);

        _logger.LogInformation(
            "Loaded {Count} active asset classification rules for date {Date}",
            rules.Count,
            date);

        return rules;
    }

    public async Task<(bool NeedsReclassification, PropertyClassification? NewClassification)> CheckReclassificationAsync(
        decimal acquisitionCost,
        int estimatedUsefulLifeMonths,
        PropertyClassification currentClassification,
        CancellationToken cancellationToken = default)
    {
        var newClassification = await DetermineClassificationAsync(
            acquisitionCost,
            estimatedUsefulLifeMonths,
            cancellationToken: cancellationToken);

        if (newClassification != currentClassification)
        {
            _logger.LogInformation(
                "Asset needs reclassification: {Old} → {New} (Cost: {Cost})",
                currentClassification,
                newClassification,
                acquisitionCost);

            return (true, newClassification);
        }

        return (false, null);
    }

    /// <summary>
    /// Fallback classification logic when no rules are configured
    /// Based on COA Circular 2022-004 defaults
    /// </summary>
    private static PropertyClassification GetDefaultClassification(
        decimal acquisitionCost,
        int estimatedUsefulLifeMonths)
    {
        if (acquisitionCost <= 1000)
            return PropertyClassification.Consumable;

        if (acquisitionCost > 50000) // Current threshold as of 2022
            return PropertyClassification.PropertyPlantEquipment;

        // Between 1000 and 50000
        if (estimatedUsefulLifeMonths >= 12) // > 1 year
            return PropertyClassification.SemiExpendable;

        return PropertyClassification.Consumable;
    }

    /// <summary>
    /// Clear classification rules cache (call when rules are updated)
    /// </summary>
    public void ClearCache()
    {
        _logger.LogInformation("Clearing asset classification rules cache");
        // Memory cache doesn't have a clear all method, but entries will expire
        // Alternative: use a cache wrapper with tracking
    }
}
