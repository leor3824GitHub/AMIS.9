using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Search.v1;

public sealed class SearchProcurementPlansCommand : PaginationFilter, IRequest<PagedList<ProcurementPlanListItemResponse>>
{
    public int? FiscalYear { get; set; }
    public Guid? DepartmentId { get; set; }
}
