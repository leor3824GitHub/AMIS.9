using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;

public sealed class GetInventoryTransactionHandler(
    [FromKeyedServices("catalog:inventory-transactions")] IReadRepository<InventoryTransaction> repository,
    ICacheService cache
) : IRequestHandler<GetInventoryTransactionRequest, InventoryTransactionResponse>
{
    public async Task<InventoryTransactionResponse> Handle(GetInventoryTransactionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"inventory-transaction:{request.Id}",
            async () =>
            {
                var transaction = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (transaction == null)
                    throw new InventoryTransactionNotFoundException(request.Id);

                return new InventoryTransactionResponse(
                    transaction.Id,
                    transaction.ProductId,
                    transaction.Qty,
                    transaction.UnitCost,
                    transaction.SourceId,
                    transaction.Location,
                    transaction.TransactionType
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
