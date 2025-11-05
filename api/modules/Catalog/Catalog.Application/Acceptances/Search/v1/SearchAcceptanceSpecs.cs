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
            .OrderBy(i => i.AcceptanceDate, !command.HasOrderBy());

        if (command.InspectionId.HasValue)
        {
            Query.Where(i => i.InspectionId == command.InspectionId.Value);
        }

        if (command.PurchaseId.HasValue)
        {
            Query.Where(i => i.PurchaseId == command.PurchaseId.Value);
        }

        if (command.FromDate.HasValue)
        {
            var from = command.FromDate.Value;
            Query.Where(i => i.AcceptanceDate >= from);
        }
        if (command.ToDate.HasValue)
        {
            var to = command.ToDate.Value;
            Query.Where(i => i.AcceptanceDate <= to);
        }
    }
}
