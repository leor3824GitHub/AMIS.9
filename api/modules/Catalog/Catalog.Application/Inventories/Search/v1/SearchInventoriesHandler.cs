using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.Inventories.Search.v1;
public sealed class SearchInventoriesHandler(
    [FromKeyedServices("catalog:inventories")] IReadRepository<Inventory> repository)
    : IRequestHandler<SearchInventoriesCommand, PagedList<InventoryResponse>>
{
    public async Task<PagedList<InventoryResponse>> Handle(SearchInventoriesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInventorySpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InventoryResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

