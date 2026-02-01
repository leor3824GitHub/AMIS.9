using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Application.PropertyCodes;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Services;

/// <summary>
/// Default property code generator: ASSET-{AcceptanceId}-{PurchaseItemId} (uppercase, no dashes)
/// Replace with COA/NFA-compliant code generator via DI in production.
/// </summary>
public sealed class DefaultAssetPropertyCodeGenerator : IAssetPropertyCodeGenerator
{
    public Task<string> GenerateAsync(AcceptanceItem item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);
        return Task.FromResult($"ASSET-{item.AcceptanceId:N}-{item.PurchaseItemId:N}".ToUpperInvariant());
    }

    public Task<string> GenerateAsync(CoaPropertyCodeRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Task.FromResult($"ASSET-{request.AcquisitionDate:yyyyMMdd}-{Guid.NewGuid():N}".ToUpperInvariant());
    }
}

