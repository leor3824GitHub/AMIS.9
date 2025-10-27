using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Specifications;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;

public class SearchAcceptanceSpecs : EntitiesByPaginationFilterSpec<Acceptance, AcceptanceResponse>
{
    public SearchAcceptanceSpecs(SearchAcceptancesCommand command)
        : base(command)
    {
        Query
            .Include(i => i.SupplyOfficer)
            .Include(i => i.Purchase)
            .OrderBy(i => i.AcceptanceDate, !command.HasOrderBy())
            .Where(i => i.InspectionId == command.InspectionId!.Value, command.InspectionId.HasValue)
            .Where(i => i.PurchaseId == command.PurchaseId!.Value, command.PurchaseId.HasValue)
            .Where(i => i.AcceptanceDate >= command.FromDate, command.FromDate.HasValue)
            .Where(i => i.AcceptanceDate <= command.ToDate, command.ToDate.HasValue);
    }
}
