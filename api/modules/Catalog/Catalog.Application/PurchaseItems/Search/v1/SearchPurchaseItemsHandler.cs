using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;
public sealed class SearchPurchaseItemsHandler(
    [FromKeyedServices("catalog:purchaseItems")] IReadRepository<PurchaseItem> repository)
    : IRequestHandler<SearchPurchaseItemsCommand, PagedList<PurchaseItemResponse>>
{
    public async Task<PagedList<PurchaseItemResponse>> Handle(SearchPurchaseItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPurchaseItemSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PurchaseItemResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

