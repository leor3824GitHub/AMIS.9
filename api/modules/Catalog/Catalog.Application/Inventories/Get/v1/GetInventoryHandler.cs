using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
public sealed class GetInventoryHandler(
    [FromKeyedServices("catalog:inventories")] IReadRepository<Inventory> repository,
    ICacheService cache)
    : IRequestHandler<GetInventoryRequest, InventoryResponse>
{
    public async Task<InventoryResponse> Handle(GetInventoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"inventory:{request.Id}",
            async () =>
            {
                var spec = new GetInventorySpecs(request.Id);
                var inventoryItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (inventoryItem == null) throw new InventoryNotFoundException(request.Id);
                return inventoryItem;
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
