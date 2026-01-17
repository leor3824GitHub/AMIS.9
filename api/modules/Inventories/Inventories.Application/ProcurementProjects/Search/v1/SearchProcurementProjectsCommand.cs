using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Search.v1;

public sealed class SearchProcurementProjectsCommand : PaginationFilter, IRequest<PagedList<ProcurementProjectListItemResponse>>
{
    public string? ProjectTitle { get; set; }
    public string? PmoEndUser { get; set; }
    public string? FundSource { get; set; }
}

