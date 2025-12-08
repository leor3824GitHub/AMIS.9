using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Services;

/// <summary>
/// Default property code generator: ASSET-{AcceptanceId}-{PurchaseItemId} (uppercase, no dashes)
/// Replace with COA/NFA-compliant code generator via DI in production.
/// </summary>
public sealed class DefaultAssetPropertyCodeGenerator : IAssetPropertyCodeGenerator
{
    public string Generate(AcceptanceItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return $"ASSET-{item.AcceptanceId:N}-{item.PurchaseItemId:N}".ToUpperInvariant();
    }
}
