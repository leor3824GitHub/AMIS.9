using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Search.v1;

public class SearchIssuancesCommand : PaginationFilter, IRequest<PagedList<IssuanceResponse>>
{
    public Guid? ProductId { get; set; }
    public Guid? EmployeeId { get; set; }
}
