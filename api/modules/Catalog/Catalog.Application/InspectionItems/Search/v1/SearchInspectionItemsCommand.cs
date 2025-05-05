using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Search.v1;

public class SearchInspectionItemsCommand : PaginationFilter, IRequest<PagedList<InspectionItemResponse>>
{
    public Guid? InspectionId { get; set; }
    public Guid? PurchaseItemId { get; set; }
}
