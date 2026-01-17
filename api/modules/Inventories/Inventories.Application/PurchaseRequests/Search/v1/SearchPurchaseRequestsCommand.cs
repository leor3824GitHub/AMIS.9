using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Search.v1;

public class SearchPurchaseRequestsCommand : PaginationFilter, IRequest<PagedList<PurchaseRequestResponse>>
{
    public PurchaseRequestStatus? Status { get; set; }
    public Guid? RequestedBy { get; set; }
}

