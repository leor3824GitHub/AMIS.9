using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Select.v1;

internal sealed class SelectLowestCanvassSpecs : Specification<Canvass>
{
    public SelectLowestCanvassSpecs(Guid purchaseRequestId)
    {
        Query.Where(c => c.PurchaseRequestId == purchaseRequestId);
    }
}
