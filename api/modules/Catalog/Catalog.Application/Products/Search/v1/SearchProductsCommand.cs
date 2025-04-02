using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Search.v1;

public class SearchProductsCommand : PaginationFilter, IRequest<PagedList<ProductResponse>>
{
    public Guid? CategoryId { get; set; }
    public string? Name { get; set; }
}
