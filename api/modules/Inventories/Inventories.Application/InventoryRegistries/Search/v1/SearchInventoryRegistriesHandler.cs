using System.Linq;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Search.v1;

public sealed class SearchInventoryRegistriesHandler(
    [FromKeyedServices("inventories:inventory-registries")] IReadRepository<InventoryRegistry> repository)
    : IRequestHandler<SearchInventoryRegistriesCommand, PagedList<InventoryRegistryResponse>>
{
    public async Task<PagedList<InventoryRegistryResponse>> Handle(SearchInventoryRegistriesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInventoryRegistriesSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = items.Select(x => new InventoryRegistryResponse(
            x.Id,
            x.PropertyCode,
            x.Description,
            x.Quantity,
            x.Status,
            x.Location,
            x.ReceivedDate,
            x.IssuedDate,
            x.LastTransactionDate,
            x.LastTransactionType,
            x.LastTransactionReference)).ToList();

        return new PagedList<InventoryRegistryResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
