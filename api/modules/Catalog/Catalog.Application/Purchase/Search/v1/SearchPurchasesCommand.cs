using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Search.v1;

public class SearchPurchasesCommand : PaginationFilter, IRequest<PagedList<PurchaseResponse>>
{
    public Guid? SupplierId { get; set; }
}
