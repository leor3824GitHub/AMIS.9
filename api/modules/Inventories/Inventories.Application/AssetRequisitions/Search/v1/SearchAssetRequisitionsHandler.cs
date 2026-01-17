using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Search.v1;

public sealed class SearchAssetRequisitionsHandler(
    [FromKeyedServices("inventories:assetrequisitions")] IReadRepository<AssetRequisition> repository)
    : IRequestHandler<SearchAssetRequisitionsCommand, PagedList<AssetRequisitionDto>>
{
    public async Task<PagedList<AssetRequisitionDto>> Handle(SearchAssetRequisitionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAssetRequisitionsSpecs(request);

        var requisitions = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AssetRequisitionDto>(requisitions, request.PageNumber, request.PageSize, totalCount);
    }
}

