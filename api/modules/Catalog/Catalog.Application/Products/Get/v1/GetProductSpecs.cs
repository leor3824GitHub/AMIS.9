using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Products.Get.v1;

public class GetProductSpecs : Specification<Product, ProductResponse>
{
    public GetProductSpecs(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand);
    }
}
