using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.Search.v1;

public sealed class SearchPhysicalAssetsHandler(
    [FromKeyedServices("catalog:physicalassets")] IReadRepository<PhysicalAsset> repository)
    : IRequestHandler<SearchPhysicalAssetsCommand, PagedList<PhysicalAssetResponse>>
{
    public async Task<PagedList<PhysicalAssetResponse>> Handle(
        SearchPhysicalAssetsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPhysicalAssetsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<PhysicalAssetResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
