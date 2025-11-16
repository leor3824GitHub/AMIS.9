using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;

public class SearchInspectionRequestsCommand : PaginationFilter, IRequest<PagedList<InspectionRequestResponse>>
{
    public Guid? PurchaseId { get; set; }
    public Guid? InspectedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    // Optional filter for statuses to include (e.g. Assigned/InProgress)
    public ICollection<InspectionRequestStatus>? Statuses { get; set; }
}
