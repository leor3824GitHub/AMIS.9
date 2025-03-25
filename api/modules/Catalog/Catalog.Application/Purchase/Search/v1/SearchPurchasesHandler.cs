using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.Purchases.Search.v1;
public sealed class SearchPurchasesHandler(
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> repository)
    : IRequestHandler<SearchPurchasesCommand, PagedList<PurchaseResponse>>
{
    public async Task<PagedList<PurchaseResponse>> Handle(SearchPurchasesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPurchaseSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PurchaseResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

