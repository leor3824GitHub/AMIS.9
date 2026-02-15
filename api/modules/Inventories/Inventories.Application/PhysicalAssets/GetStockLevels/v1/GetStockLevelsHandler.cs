using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.GetStockLevels.v1;

public sealed class GetStockLevelsHandler(
    [FromKeyedServices("inventories:physicalassets")] IReadRepository<PhysicalAsset> repository)
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

        // Location tracking removed - CurrentAssignment navigation no longer available
        var byLocation = new Dictionary<string, int>();

        return new GetStockLevelsResponse(
            totalAssets,
            disposedAssets,
            activeAssets,
            byClassification,
            byCondition,
            byLocation);
    }
}

