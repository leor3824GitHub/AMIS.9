using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;

public class GetInventorySpecs : Specification<Inventory, InventoryResponse>
{
    public GetInventorySpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Product)
            .ThenInclude(p => p.Category);
        
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


