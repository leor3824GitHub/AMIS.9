using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.Inventories.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inventories.Search.v1;

public class SearchInventoriesCommand : PaginationFilter, IRequest<PagedList<InventoryResponse>>
{
    public Guid? ProductId { get; set; }
}

