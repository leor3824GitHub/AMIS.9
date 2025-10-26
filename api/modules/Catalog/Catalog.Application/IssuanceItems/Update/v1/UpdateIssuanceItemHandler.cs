using System.Transactions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Update.v1;
public sealed class UpdateIssuanceItemHandler(
    ILogger<UpdateIssuanceItemHandler> logger,
    [FromKeyedServices("catalog:issuanceItems")] IRepository<IssuanceItem> repository,
    [FromKeyedServices("catalog:inventories")] IRepository<Inventory> inventoryRepository,
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> issuanceRepository)
    : IRequestHandler<UpdateIssuanceItemCommand, UpdateIssuanceItemResponse>
{
    public async Task<UpdateIssuanceItemResponse> Handle(UpdateIssuanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var issuanceItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new IssuanceItemNotFoundException(request.Id);

        var issuance = await issuanceRepository.GetByIdAsync(request.IssuanceId, cancellationToken)
            ?? throw new IssuanceNotFoundException(request.IssuanceId);

        if (issuance.IsClosed)
        {
            return new UpdateIssuanceItemResponse(null, false, "Issuance is closed and cannot be modified.");
        }

        var spec = new GetInventoryProductIdSpecs(request.ProductId);
        var inventory = await inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inventory == null)
        {
            logger.LogWarning("Inventory not found for ProductId {ProductId}", request.ProductId);
            return new UpdateIssuanceItemResponse(Id: null, Success: false, ErrorMessage: "Inventory not found.");
        }

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var originalQty = issuanceItem.Qty;
        var originalUnitPrice = issuanceItem.UnitPrice;

        issuanceItem.Update(request.IssuanceId, request.ProductId, request.Qty, request.UnitPrice, request.Status);
        await repository.UpdateAsync(issuanceItem, cancellationToken);
        logger.LogInformation("Issuance item {IssuanceItemId} updated.", issuanceItem.Id);

        inventory.UpdateStock(originalQty, request.Qty, request.UnitPrice);
        await inventoryRepository.UpdateAsync(inventory, cancellationToken);
        logger.LogInformation("Inventory updated for ProductId {ProductId}, updated {Qty} units.", request.ProductId, request.Qty);

        issuance.RegisterItemUpdated(originalQty, originalUnitPrice, request.Qty, request.UnitPrice);
        await issuanceRepository.UpdateAsync(issuance, cancellationToken);
        logger.LogInformation("Issuance {IssuanceId} total recalculated to {TotalAmount}", issuance.Id, issuance.TotalAmount);

        scope.Complete();

        return new UpdateIssuanceItemResponse(Id: issuanceItem.Id, Success: true, ErrorMessage: null);
    }
}
