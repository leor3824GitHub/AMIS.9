using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inventories.Search.v1;
public class SearchInventorySpecs : EntitiesByPaginationFilterSpec<Inventory, InventoryResponse>
{
    public SearchInventorySpecs(SearchInventoriesCommand command)
        : base(command)
    {
        Query.Include(p => p.Product);

        if (command.ProductId.HasValue)
        {
            Query.Where(p => p.ProductId == command.ProductId.Value);
        }
    }
}
