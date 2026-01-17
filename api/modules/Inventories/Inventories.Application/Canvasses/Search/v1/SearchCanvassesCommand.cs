using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.Canvasses.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Search.v1;

public class SearchCanvassesCommand : PaginationFilter, IRequest<PagedList<CanvassResponse>>
{
    public Guid? PurchaseRequestId { get; set; }
    public Guid? SupplierId { get; set; }
    public bool? IsSelected { get; set; }
}

