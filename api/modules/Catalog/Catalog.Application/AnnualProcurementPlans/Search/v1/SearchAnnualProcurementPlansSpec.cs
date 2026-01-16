using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Search.v1;

public sealed class SearchAnnualProcurementPlansSpec : EntitiesByPaginationFilterSpec<AnnualProcurementPlanHeader, AnnualProcurementPlanListItemResponse>
{
    public SearchAnnualProcurementPlansSpec(SearchAnnualProcurementPlansCommand request)
        : base(request)
    {
        Query
            .OrderByDescending(x => x.Created, !request.HasOrderBy())
            .Where(x => x.FiscalYear == request.FiscalYear!.Value, request.FiscalYear.HasValue)
            .Where(x => x.Items.Any(i => i.DepartmentId == request.DepartmentId!.Value), request.DepartmentId.HasValue);
    }
}
