using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inspections.Get.v1;

public class GetInspectionSpecs : Specification<Inspection, InspectionResponse>
{
    public GetInspectionSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.Purchase)
            .Include(i => i.InspectorId);
    }
}
