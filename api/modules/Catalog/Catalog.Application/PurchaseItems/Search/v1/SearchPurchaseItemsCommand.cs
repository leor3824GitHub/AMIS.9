using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;

public class SearchPurchaseItemsCommand : PaginationFilter, IRequest<PagedList<PurchaseItemResponse>>
{
    public Guid? PurchaseId { get; set; }
    public Guid? ProductId { get; set; }
}
