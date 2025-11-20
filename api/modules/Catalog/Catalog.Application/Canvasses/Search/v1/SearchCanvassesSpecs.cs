using Ardalis.Specification;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Search.v1;

public sealed class SearchCanvassesSpecs : Specification<Canvass>
{
    public SearchCanvassesSpecs(SearchCanvassesCommand command)
    {
        Query.OrderByDescending(c => c.ResponseDate);

        if (command.PurchaseRequestId.HasValue)
        {
            Query.Where(c => c.PurchaseRequestId == command.PurchaseRequestId.Value);
        }

        if (command.SupplierId.HasValue)
        {
            Query.Where(c => c.SupplierId == command.SupplierId.Value);
        }

        if (command.IsSelected.HasValue)
        {
            Query.Where(c => c.IsSelected == command.IsSelected.Value);
        }
    }
}
