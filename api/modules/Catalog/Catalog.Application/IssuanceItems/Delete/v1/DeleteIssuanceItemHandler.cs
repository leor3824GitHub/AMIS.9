using System.Transactions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Delete.v1;
public sealed class DeleteIssuanceItemHandler(
    ILogger<DeleteIssuanceItemHandler> logger,
    [FromKeyedServices("catalog:issuanceItems")] IRepository<IssuanceItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository,
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> issuanceRepository)
    : IRequestHandler<DeleteIssuanceItemCommand>
{
    public async Task Handle(DeleteIssuanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Retrieve the issuance item from the repository
        var issuanceItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new IssuanceItemNotFoundException(request.Id);

        var issuance = await issuanceRepository.GetByIdAsync(issuanceItem.IssuanceId, cancellationToken)
            ?? throw new IssuanceNotFoundException(issuanceItem.IssuanceId);

        if (issuance.IsClosed)
        {
            logger.LogWarning("Attempted to delete issuance item {IssuanceItemId} from closed issuance {IssuanceId}", issuanceItem.Id, issuance.Id);
            return;
        }

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await repository.DeleteAsync(issuanceItem, cancellationToken);
        logger.LogInformation("IssuanceItem with id : {IssuanceItemId} deleted successfully", issuanceItem.Id);

        // Check if we need to update the inventory
        var spec = new GetInventoryProductIdSpecs(issuanceItem.ProductId);
        var inventory = await inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inventory == null)
        {
            // Log a warning if inventory is not found for the product
            logger.LogWarning("Inventory not found for ProductId {ProductId}", issuanceItem.ProductId);
        }
        else
        {
            try
            {
                // Update the stock based on the deleted issuance item quantity
                inventory.UpdateStock(issuanceItem.Qty);

                // Save the updated inventory
                await inventoryRepository.UpdateAsync(inventory, cancellationToken);
                logger.LogInformation("{Qty} quantity of ProductId {ProductId} returned to inventory successfully.", issuanceItem.Qty, issuanceItem.ProductId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating inventory for ProductId {ProductId}", issuanceItem.ProductId);
            }
        }

        issuance.RegisterItemRemoved(issuanceItem.Qty, issuanceItem.UnitPrice);
        await issuanceRepository.UpdateAsync(issuance, cancellationToken);
        logger.LogInformation("Issuance {IssuanceId} total recalculated to {TotalAmount}", issuance.Id, issuance.TotalAmount);

        scope.Complete();
    }

}
