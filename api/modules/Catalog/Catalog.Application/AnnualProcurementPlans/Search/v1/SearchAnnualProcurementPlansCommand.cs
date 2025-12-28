using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Search.v1;

public sealed class SearchAnnualProcurementPlansCommand : PaginationFilter, IRequest<PagedList<AnnualProcurementPlanListItemResponse>>
{
    public int? FiscalYear { get; set; }
    public Guid? DepartmentId { get; set; }
}
