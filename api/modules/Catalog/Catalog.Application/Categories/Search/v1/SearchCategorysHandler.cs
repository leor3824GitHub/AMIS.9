using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Categories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Categories.Search.v1;
public sealed class SearchCategorysHandler(
    [FromKeyedServices("catalog:categories")] IReadRepository<Category> repository)
    : IRequestHandler<SearchCategorysCommand, PagedList<CategoryResponse>>
{
    public async Task<PagedList<CategoryResponse>> Handle(SearchCategorysCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCategorySpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CategoryResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
