using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;

namespace AMIS.WebApi.Inventories.Application.SemexRegistry.Search.v1;

public sealed class SearchSemexRegistriesSpec : Specification<SemexRegistryDomain>
{
    public SearchSemexRegistriesSpec(SearchSemexRegistriesCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.ItemCode))
            Query.Where(r => r.ItemCode.Contains(request.ItemCode, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Description))
            Query.Where(r => r.Description.Contains(request.Description, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Location))
            Query.Where(r => r.Location.Contains(request.Location, System.StringComparison.OrdinalIgnoreCase));

        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        Query.OrderByDescending(r => r.Created);
        Query.Skip((pageNumber - 1) * pageSize);
        Query.Take(pageSize);
    }
}
