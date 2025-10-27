using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Purchases.Search.v1;
public class SearchPurchaseSpecs : EntitiesByPaginationFilterSpec<Purchase, PurchaseResponse>
{
    public SearchPurchaseSpecs(SearchPurchasesCommand command)
        : base(command) =>
        Query
            .Include(p => p.Supplier)
            .Include(o => o.Items)
            .OrderBy(c => c.PurchaseDate, !command.HasOrderBy())
            .Where(p => p.SupplierId == command.SupplierId!.Value, command.SupplierId.HasValue);
}
