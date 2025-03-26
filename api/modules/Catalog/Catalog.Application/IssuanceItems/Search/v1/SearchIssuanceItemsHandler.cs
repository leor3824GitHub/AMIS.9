using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Search.v1;
public sealed class SearchIssuanceItemsHandler(
    [FromKeyedServices("catalog:issuanceItems")] IReadRepository<IssuanceItem> repository)
    : IRequestHandler<SearchIssuanceItemsCommand, PagedList<IssuanceItemResponse>>
{
    public async Task<PagedList<IssuanceItemResponse>> Handle(SearchIssuanceItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchIssuanceItemSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<IssuanceItemResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

