using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Delete.v1;

public sealed class DeleteInventoryTransactionHandler(
    ILogger<DeleteInventoryTransactionHandler> logger,
    [FromKeyedServices("catalog:inventory-transactions")] IRepository<InventoryTransaction> repository
) : IRequestHandler<DeleteInventoryTransactionCommand, DeleteInventoryTransactionResponse>
{
    public async Task<DeleteInventoryTransactionResponse> Handle(DeleteInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"InventoryTransaction with Id {request.Id} not found.");

        await repository.DeleteAsync(transaction, cancellationToken);
        logger.LogInformation("Inventory transaction deleted: {TransactionId}", transaction.Id);

        return new DeleteInventoryTransactionResponse(transaction.Id);
    }
}
