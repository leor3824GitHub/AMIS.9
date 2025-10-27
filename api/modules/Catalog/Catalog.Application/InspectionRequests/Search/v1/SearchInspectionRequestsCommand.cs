using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;

public class SearchInspectionRequestsCommand : PaginationFilter, IRequest<PagedList<InspectionRequestResponse>>
{
    public Guid? PurchaseId { get; set; }
    public Guid? InspectedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
