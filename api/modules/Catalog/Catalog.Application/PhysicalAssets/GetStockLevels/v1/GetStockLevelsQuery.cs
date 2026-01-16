using MediatR;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.GetStockLevels.v1;

public sealed record GetStockLevelsQuery : IRequest<GetStockLevelsResponse>;

public sealed record GetStockLevelsResponse(
    int TotalAssets,
    int DisposedAssets,
    int ActiveAssets,
    Dictionary<string, int> AssetsByClassification,
    Dictionary<string, int> AssetsByCondition,
    Dictionary<string, int> AssetsByLocation);
