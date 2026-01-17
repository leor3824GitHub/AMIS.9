using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Application.Inventories.Get.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Inventories.Search.v1;
public class SearchInventorySpecs : EntitiesByPaginationFilterSpec<Inventory, InventoryResponse>
{
    public SearchInventorySpecs(SearchInventoriesCommand command)
        : base(command) =>
        Query
            .Include(p => p.Product)
            .Where(p => p.ProductId == command.ProductId!.Value, command.ProductId.HasValue);
}

