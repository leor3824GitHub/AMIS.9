using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Search.v1;

public sealed class SearchPpeCategoryCodesCommand : PaginationFilter, IRequest<PagedList<PpeCategoryCodeResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? AccountCode { get; set; }
    public bool? IsActive { get; set; }
}
