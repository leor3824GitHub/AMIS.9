using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.Issuances.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Issuances.Search.v1;

public class SearchIssuancesCommand : PaginationFilter, IRequest<PagedList<IssuanceResponse>>
{
    public Guid? EmployeeId { get; set; }
}

