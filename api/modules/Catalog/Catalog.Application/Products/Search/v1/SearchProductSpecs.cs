using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Products.Search.v1;
public class SearchProductSpecs : EntitiesByPaginationFilterSpec<Product, ProductResponse>
{
    public SearchProductSpecs(SearchProductsCommand command)
        : base(command) =>
        Query
            .Include(p => p.Brand)
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(p => p.BrandId == command.BrandId!.Value, command.BrandId.HasValue)
            .Where(p => p.Price >= command.MinimumRate!.Value, command.MinimumRate.HasValue)
            .Where(p => p.Price <= command.MaximumRate!.Value, command.MaximumRate.HasValue);
}
