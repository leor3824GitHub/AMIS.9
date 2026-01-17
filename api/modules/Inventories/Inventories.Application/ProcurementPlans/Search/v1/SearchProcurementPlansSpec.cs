using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Search.v1;

public sealed class SearchProcurementPlansSpec : EntitiesByPaginationFilterSpec<ProcurementPlanHeader, ProcurementPlanListItemResponse>
{
    public SearchProcurementPlansSpec(SearchProcurementPlansCommand request)
        : base(request)
    {
        Query
            .OrderByDescending(x => x.Created, !request.HasOrderBy())
            .Where(x => x.FiscalYear == request.FiscalYear!.Value, request.FiscalYear.HasValue)
            .Where(x => x.DepartmentId == request.DepartmentId!.Value, request.DepartmentId.HasValue);
    }
}

