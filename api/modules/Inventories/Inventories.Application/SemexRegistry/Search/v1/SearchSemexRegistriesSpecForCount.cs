using Ardalis.Specification;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;

namespace AMIS.WebApi.Inventories.Application.SemexRegistry.Search.v1;

public sealed class SearchSemexRegistriesSpecForCount : Specification<SemexRegistryDomain>
{
    public SearchSemexRegistriesSpecForCount(SearchSemexRegistriesCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.ItemCode))
            Query.Where(r => r.ItemCode.Contains(request.ItemCode, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Description))
            Query.Where(r => r.Description.Contains(request.Description, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Location))
            Query.Where(r => r.Location.Contains(request.Location, System.StringComparison.OrdinalIgnoreCase));
    }
}
