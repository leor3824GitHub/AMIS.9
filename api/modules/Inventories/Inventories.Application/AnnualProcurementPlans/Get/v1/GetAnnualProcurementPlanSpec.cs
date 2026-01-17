using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;

public sealed class GetAnnualProcurementPlanSpec : Specification<AnnualProcurementPlanHeader>
{
    public GetAnnualProcurementPlanSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Items);
    }
}

