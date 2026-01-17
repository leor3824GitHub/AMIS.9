using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Select.v1;

internal sealed class SelectLowestCanvassSpecs : Specification<Canvass>
{
    public SelectLowestCanvassSpecs(Guid purchaseRequestId)
    {
        Query.Where(c => c.PurchaseRequestId == purchaseRequestId);
    }
}

