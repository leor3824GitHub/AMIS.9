using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Search.v1;

public class SearchSuppliersCommand : PaginationFilter, IRequest<PagedList<SupplierResponse>>
{
    public string? Name { get; set; }
    public string? Address { get; set; }

}
