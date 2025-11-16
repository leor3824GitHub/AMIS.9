using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Search.v1;

public class SearchPurchaseRequestsCommand : PaginationFilter, IRequest<PagedList<PurchaseRequestResponse>>
{
    public PurchaseRequestStatus? Status { get; set; }
    public Guid? RequestedBy { get; set; }
}
