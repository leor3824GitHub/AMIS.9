using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.ReserveStock.v1;

internal sealed class ReserveStockHandler(
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<ReserveStockCommand, ReserveStockResponse>
{
    public async Task<ReserveStockResponse> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var inventory = await repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.ReserveStock(request.Quantity);

        await repository.SaveChangesAsync(cancellationToken);

        return new ReserveStockResponse(
            inventory.Id,
            inventory.ReservedQty,
            inventory.AvailableQty,
            $"Successfully reserved {request.Quantity} units. Available: {inventory.AvailableQty}");
    }
}
