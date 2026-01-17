using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Application.Products.Get.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Products.Search.v1;
public class SearchProductSpecs : EntitiesByPaginationFilterSpec<Product, ProductResponse>
{
    public SearchProductSpecs(SearchProductsCommand command)
        : base(command) =>
        Query
            .Include(p => p.Category)
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(p => p.CategoryId == command.CategoryId!.Value, command.CategoryId.HasValue);
}

