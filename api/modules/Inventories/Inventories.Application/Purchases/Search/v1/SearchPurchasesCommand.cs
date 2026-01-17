using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.Purchases.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Purchases.Search.v1;

public class SearchPurchasesCommand : PaginationFilter, IRequest<PagedList<PurchaseResponse>>
{
    public Guid? SupplierId { get; set; }

    // When true, exclude purchases that already have an inspection request
    public bool OnlyWithoutInspectionRequest { get; set; }
}

