using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Brands.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Brands.Search.v1;

public class SearchBrandsCommand : PaginationFilter, IRequest<PagedList<BrandResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
