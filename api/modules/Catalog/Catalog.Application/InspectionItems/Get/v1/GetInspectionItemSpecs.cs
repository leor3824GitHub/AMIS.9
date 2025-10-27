using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;

public class GetInspectionItemSpecs : Specification<InspectionItem, InspectionItemResponse>
{
    public GetInspectionItemSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.PurchaseItem);
    }
}
