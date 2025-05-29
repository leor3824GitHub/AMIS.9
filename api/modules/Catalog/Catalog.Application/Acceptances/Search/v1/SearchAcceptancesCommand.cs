using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;

public class SearchAcceptancesCommand : PaginationFilter, IRequest<PagedList<AcceptanceResponse>>
{
    public Guid? PurchaseId { get; set; }
    public Guid? AcceptedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
