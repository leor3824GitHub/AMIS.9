using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inventories.RecordCycleCount.v1;

public class RecordCycleCountHandler(
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> repository)
    : IRequestHandler<RecordCycleCountCommand, RecordCycleCountResponse>
{
    public async Task<RecordCycleCountResponse> Handle(RecordCycleCountCommand request, CancellationToken cancellationToken)
    {
        var inventory = await repository.GetByIdAsync(request.InventoryId, cancellationToken);

        if (inventory == null)
            throw new InvalidOperationException($"Inventory with ID {request.InventoryId} not found.");

        int previousQty = inventory.Qty;
        inventory.RecordCycleCount(request.CountedQty, request.CountDate);
        await repository.SaveChangesAsync(cancellationToken);

        int variance = request.CountedQty - previousQty;
        string message = variance == 0 
            ? "Cycle count recorded - no variance detected."
            : $"Cycle count recorded - variance of {variance} units detected.";

        return new RecordCycleCountResponse(
            inventory.Id,
            previousQty,
            request.CountedQty,
            variance,
            inventory.StockStatus,
            request.CountDate,
            message
        );
    }
}
