using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Create.v1;

public sealed class CreateInventoryTransactionHandler(
    ILogger<CreateInventoryTransactionHandler> logger,
    [FromKeyedServices("catalog:inventory-transactions")] IRepository<InventoryTransaction> repository
) : IRequestHandler<CreateInventoryTransactionCommand, CreateInventoryTransactionResponse>
{
    public async Task<CreateInventoryTransactionResponse> Handle(CreateInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transaction = InventoryTransaction.Create(
            productId: request.ProductId,
            qty: request.Qty,
            purchasePrice: request.UnitCost,
            sourceId: request.SourceId,
            location: request.Location,
            transactionType: request.TransactionType
        );

        await repository.AddAsync(transaction, cancellationToken);

        logger.LogInformation("Inventory transaction created: {TransactionId}", transaction.Id);

        return new CreateInventoryTransactionResponse(transaction.Id);
    }
}

