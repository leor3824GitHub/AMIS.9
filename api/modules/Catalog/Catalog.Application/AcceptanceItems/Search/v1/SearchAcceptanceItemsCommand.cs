using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Search.v1;

public class SearchAcceptanceItemsCommand : PaginationFilter, IRequest<PagedList<AcceptanceItemResponse>>
{
    public Guid? AcceptanceId { get; set; }
    public Guid? PurchaseItemId { get; set; }
}
