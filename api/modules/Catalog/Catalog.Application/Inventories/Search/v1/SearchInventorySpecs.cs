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
        Query
            .Include(p => p.Product)
            .ThenInclude(p => p.Category);

        if (command.ProductId.HasValue)
        {
            Query.Where(p => p.ProductId == command.ProductId.Value);
        }
        
        Query.Select(i => new InventoryResponse(
            i.Id,
            i.ProductId!.Value,
            i.Qty,
            i.AvePrice,
            i.StockStatus,
            i.ReservedQty,
            i.Location,
            i.Product != null ? new Products.Get.v1.ProductResponse(
                i.Product.Id,
                i.Product.Name,
                i.Product.Description,
                i.Product.Sku,
                i.Product.Unit,
                i.Product.ImagePath,
                i.Product.CategoryId,
                i.Product.Category != null ? new Categories.Get.v1.CategoryResponse(
                    i.Product.Category.Id,
                    i.Product.Category.Name,
                    i.Product.Category.Description
                ) : null
            ) : null
        ));
    }
}

