using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Get.v1;

public sealed class GetProcurementPlanSpec : Specification<ProcurementPlanHeader>
{
    public GetProcurementPlanSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Items);
    }
}
