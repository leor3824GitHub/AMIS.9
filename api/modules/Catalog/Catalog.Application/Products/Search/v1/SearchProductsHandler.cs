using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.Products.Search.v1;
public sealed class SearchProductsHandler(
    [FromKeyedServices("catalog:products")] IReadRepository<Product> repository)
    : IRequestHandler<SearchProductsCommand, PagedList<ProductResponse>>
{
    public async Task<PagedList<ProductResponse>> Handle(SearchProductsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProductSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ProductResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
