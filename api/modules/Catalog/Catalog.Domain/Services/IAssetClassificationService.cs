using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Service for determining asset classification based on configurable COA/DBM rules
/// </summary>
public interface IAssetClassificationService
{
    /// <summary>
    /// Determine asset classification based on cost and useful life using active rules
    /// </summary>
    Task<PropertyClassification> DetermineClassificationAsync(
        decimal acquisitionCost,
        int estimatedUsefulLifeMonths,
        DateTime? effectiveDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all active classification rules effective on a specific date
    /// </summary>
    Task<IEnumerable<AssetClassificationRule>> GetActiveRulesAsync(
        DateTime? effectiveDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if an asset needs reclassification based on current rules
    /// </summary>
    Task<(bool NeedsReclassification, PropertyClassification? NewClassification)> CheckReclassificationAsync(
        decimal acquisitionCost,
        int estimatedUsefulLifeMonths,
        PropertyClassification currentClassification,
        CancellationToken cancellationToken = default);
}
