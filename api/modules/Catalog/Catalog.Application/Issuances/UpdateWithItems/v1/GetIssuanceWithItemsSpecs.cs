using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;

internal sealed class GetIssuanceWithItemsSpecs : Specification<Issuance>, ISingleResultSpecification<Issuance>
{
    public GetIssuanceWithItemsSpecs(Guid id)
    {
        Query.AsTracking()
             .Where(x => x.Id == id)
             .Include(x => x.Items);
    }
}