using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Get.v1;

public sealed class GetAnnualProcurementPlanSpec : Specification<AnnualProcurementPlanHeader>
{
    public GetAnnualProcurementPlanSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Items);
    }
}
