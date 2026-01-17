using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Search.v1;

public sealed class SearchAnnualProcurementPlansCommand : PaginationFilter, IRequest<PagedList<AnnualProcurementPlanListItemResponse>>
{
    public int? FiscalYear { get; set; }
    public Guid? DepartmentId { get; set; }
}

