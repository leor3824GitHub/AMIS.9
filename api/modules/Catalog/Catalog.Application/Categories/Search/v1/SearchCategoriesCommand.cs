using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Categories.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Categories.Search.v1;

public class SearchCategorysCommand : PaginationFilter, IRequest<PagedList<CategoryResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
