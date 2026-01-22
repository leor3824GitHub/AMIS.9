using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Search.v1;

public sealed class SearchInventoryRegistriesSpecs : EntitiesByPaginationFilterSpec<InventoryRegistry>
{
    public SearchInventoryRegistriesSpecs(SearchInventoryRegistriesCommand command)
        : base(command)
    {
        Query
            .Where(x => x.PropertyCode == command.PropertyCode, !string.IsNullOrWhiteSpace(command.PropertyCode))
            .Where(x => x.Description.Contains(command.Description), !string.IsNullOrWhiteSpace(command.Description))
            .Where(x => x.Status == command.Status, command.Status.HasValue)
            .Where(x => x.Location == command.Location, !string.IsNullOrWhiteSpace(command.Location))
            .OrderByDescending(x => x.LastTransactionDate);
    }
}
