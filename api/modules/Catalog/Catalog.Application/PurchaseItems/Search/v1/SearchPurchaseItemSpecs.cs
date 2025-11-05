using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;
public class SearchPurchaseItemSpecs : EntitiesByPaginationFilterSpec<PurchaseItem, PurchaseItemResponse>
{
    public SearchPurchaseItemSpecs(SearchPurchaseItemsCommand command)
        : base(command)
    {
        Query
            .Include(p => p.Product)
            .OrderBy(c => c.ItemStatus, !command.HasOrderBy());

        if (command.PurchaseId.HasValue)
        {
            Query.Where(p => p.PurchaseId == command.PurchaseId.Value);
        }
    }
}
