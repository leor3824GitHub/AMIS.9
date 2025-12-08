using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Services;

/// <summary>
/// Default resolver that applies a simple 50k PPE threshold. Replace via DI with config-driven rules (e.g., RCA thresholds).
/// </summary>
public sealed class DefaultAssetClassificationResolver : IAssetClassificationResolver
{
    private const decimal PpeThreshold = 50000m;

    public PropertyClassification ResolveClassification(PurchaseItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.UnitPrice >= PpeThreshold
            ? PropertyClassification.PropertyPlantEquipment
            : PropertyClassification.SemiExpendable;
    }

    public string? ResolvePpeType(PurchaseItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.UnitPrice >= PpeThreshold ? "Equipment" : null;
    }

    public int ResolveEstimatedUsefulLifeMonths(PurchaseItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.UnitPrice >= PpeThreshold ? 60 : 12;
    }

    public string ResolveDescription(PurchaseItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Product?.Name ?? "Accepted Item";
    }

    public string ResolveUnitOfMeasure(PurchaseItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.Product?.Unit ?? "piece";
    }
}
