using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Services;

public interface IAssetClassificationResolver
{
    PropertyClassification ResolveClassification(PurchaseItem item);
    string? ResolvePpeType(PurchaseItem item);
    int ResolveEstimatedUsefulLifeMonths(PurchaseItem item);
    string ResolveDescription(PurchaseItem item);
    string ResolveUnitOfMeasure(PurchaseItem item);
}

