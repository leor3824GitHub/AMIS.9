using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;

public sealed class GetInventoryRegistryHandler(
    [FromKeyedServices("inventories:inventory-registries")] IReadRepository<InventoryRegistry> repository)
    : IRequestHandler<GetInventoryRegistryRequest, InventoryRegistryResponse>
{
    public async Task<InventoryRegistryResponse> Handle(GetInventoryRegistryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetInventoryRegistrySpecs(request.Id);
        var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            throw new InvalidOperationException($"Inventory registry with Id {request.Id} was not found.");
        }

        return new InventoryRegistryResponse(
            entity.Id,
            entity.PropertyCode,
            entity.Description,
            entity.Quantity,
            entity.Status,
            entity.Location,
            entity.ReceivedDate,
            entity.IssuedDate,
            entity.LastTransactionDate,
            entity.LastTransactionType,
            entity.LastTransactionReference);
    }
}
