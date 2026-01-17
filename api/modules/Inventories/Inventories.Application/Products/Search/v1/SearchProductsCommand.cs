using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.Products.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Products.Search.v1;

public class SearchProductsCommand : PaginationFilter, IRequest<PagedList<ProductResponse>>
{
    public Guid? CategoryId { get; set; }
}

