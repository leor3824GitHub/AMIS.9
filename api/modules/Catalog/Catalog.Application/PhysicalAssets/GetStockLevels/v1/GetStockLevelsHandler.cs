using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.GetStockLevels.v1;

public sealed class GetStockLevelsHandler(
    [FromKeyedServices("catalog:physicalassets")] IReadRepository<PhysicalAsset> repository)
    : IRequestHandler<GetStockLevelsQuery, GetStockLevelsResponse>
{
    public async Task<GetStockLevelsResponse> Handle(GetStockLevelsQuery request, CancellationToken cancellationToken)
    {
        var assets = await repository.ListAsync(cancellationToken);

        var totalAssets = assets.Count;
        var disposedAssets = assets.Count(a => a.IsDisposed);
        var activeAssets = totalAssets - disposedAssets;

        var byClassification = assets
            .Where(a => !a.IsDisposed)
            .GroupBy(a => a.CurrentClassification.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var byCondition = assets
            .Where(a => !a.IsDisposed)
            .GroupBy(a => a.Condition)
            .ToDictionary(g => g.Key, g => g.Count());

        var byLocation = assets
            .Where(a => !a.IsDisposed && !string.IsNullOrEmpty(a.Location))
            .GroupBy(a => a.Location!)
            .ToDictionary(g => g.Key, g => g.Count());

        return new GetStockLevelsResponse(
            totalAssets,
            disposedAssets,
            activeAssets,
            byClassification,
            byCondition,
            byLocation);
    }
}
