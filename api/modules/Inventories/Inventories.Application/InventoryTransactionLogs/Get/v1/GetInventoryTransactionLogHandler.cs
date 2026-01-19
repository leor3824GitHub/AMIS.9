using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;

public sealed class GetInventoryTransactionLogHandler(
    [FromKeyedServices("inventories:inventory-transaction-logs")] IReadRepository<InventoryTransactionLog> repository)
    : IRequestHandler<GetInventoryTransactionLogRequest, InventoryTransactionLogResponse>
{
    public async Task<InventoryTransactionLogResponse> Handle(GetInventoryTransactionLogRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetInventoryTransactionLogSpecs(request.Id);
        var entity = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            throw new InvalidOperationException($"Inventory transaction log with Id {request.Id} was not found.");
        }

        return new InventoryTransactionLogResponse(
            entity.Id,
            entity.PropertyCode,
            entity.TransactionType,
            entity.ReportNumber,
            entity.QuantityChange,
            entity.InventoryBefore,
            entity.InventoryAfter,
            entity.StatusBefore,
            entity.StatusAfter,
            entity.Success,
            entity.ErrorMessage,
            entity.InitiatedBy,
            entity.TransactionDate);
    }
}
