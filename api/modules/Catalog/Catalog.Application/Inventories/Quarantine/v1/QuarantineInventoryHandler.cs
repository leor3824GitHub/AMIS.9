using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.Quarantine.v1;

internal sealed class QuarantineInventoryHandler(
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<QuarantineInventoryCommand, QuarantineInventoryResponse>
{
    public async Task<QuarantineInventoryResponse> Handle(QuarantineInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await repository.GetByIdAsync(request.InventoryId, cancellationToken)
            ?? throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        inventory.MarkAsQuarantined(request.Location);

        await repository.SaveChangesAsync(cancellationToken);

        return new QuarantineInventoryResponse(
            inventory.Id,
            inventory.StockStatus.ToString(),
            inventory.Location,
            $"Inventory quarantined successfully.");
    }
}
