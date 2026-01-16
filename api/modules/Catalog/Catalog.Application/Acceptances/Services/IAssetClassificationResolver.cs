using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Services;

public interface IAssetClassificationResolver
{
    PropertyClassification ResolveClassification(PurchaseItem item);
    string? ResolvePpeType(PurchaseItem item);
    int ResolveEstimatedUsefulLifeMonths(PurchaseItem item);
    string ResolveDescription(PurchaseItem item);
    string ResolveUnitOfMeasure(PurchaseItem item);
}
