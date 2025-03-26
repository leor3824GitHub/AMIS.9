using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;
public class SearchPurchaseItemSpecs : EntitiesByPaginationFilterSpec<PurchaseItem, PurchaseItemResponse>
{
    public SearchPurchaseItemSpecs(SearchPurchaseItemsCommand command)
        : base(command) =>
        Query
            .Include(p => p.Product)
            .OrderBy(c => c.Product.Name, !command.HasOrderBy())
            .Where(p => command.PurchaseId.HasValue && p.PurchaseId == command.PurchaseId.Value)
            .Where(p => command.ProductId.HasValue && p.ProductId == command.ProductId.Value);
}
