using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Search.v1;

public class SearchInspectionsCommand : PaginationFilter, IRequest<PagedList<InspectionResponse>>
{
    public Guid? PurchaseId { get; set; }
    public Guid? InspectedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
