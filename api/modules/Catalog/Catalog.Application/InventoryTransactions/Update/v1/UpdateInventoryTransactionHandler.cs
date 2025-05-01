using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Update.v1;

public sealed class UpdateInventoryTransactionHandler(
    ILogger<UpdateInventoryTransactionHandler> logger,
    [FromKeyedServices("catalog:inventory-transactions")] IRepository<InventoryTransaction> repository
) : IRequestHandler<UpdateInventoryTransactionCommand, UpdateInventoryTransactionResponse>
{
    public async Task<UpdateInventoryTransactionResponse> Handle(UpdateInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"InventoryTransaction with Id {request.Id} not found.");

        existing.Update(
            productId: request.ProductId,
            qty: request.Qty,
            purchasePrice: request.UnitCost,
            sourceId: request.SourceId,
            location: request.Location,
            transactionType: request.TransactionType
        );

        await repository.UpdateAsync(existing, cancellationToken);
        logger.LogInformation("Inventory transaction updated: {TransactionId}", existing.Id);

        return new UpdateInventoryTransactionResponse(existing.Id);
    }
}
