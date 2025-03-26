using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Search.v1;

public class SearchIssuanceItemsCommand : PaginationFilter, IRequest<PagedList<IssuanceItemResponse>>
{
    public Guid? IssuanceId { get; set; }
    public Guid? ProductId { get; set; }
}
