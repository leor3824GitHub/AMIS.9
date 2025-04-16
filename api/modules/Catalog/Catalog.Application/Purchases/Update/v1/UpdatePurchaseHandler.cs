using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;

public sealed class UpdatePurchaseHandler(
    ILogger<UpdatePurchaseHandler> logger,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<UpdatePurchaseCommand, UpdatePurchaseResponse>
{
    public async Task<UpdatePurchaseResponse> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var (itemsToAdd, itemsToUpdateOrDelete) = SplitItems(request.Items);

        Guid resultId;

        if (itemsToAdd.Any() || itemsToAdd.Count > 0)
            resultId = await AddPurchaseWithItemsAsync(request, itemsToAdd, cancellationToken);
        else if (itemsToUpdateOrDelete.Any() || itemsToUpdateOrDelete.Count > 0)
            resultId = await UpdatePurchaseItemsAsync(request, itemsToUpdateOrDelete, cancellationToken);
        else
            resultId = request.Id; // No changes

        return new UpdatePurchaseResponse(resultId);
    }

    private static (List<PurchaseItemUpdateDto> Add, List<PurchaseItemUpdateDto> UpdateDelete)
        SplitItems(ICollection<PurchaseItemUpdateDto>? items)
    {
        var add = new List<PurchaseItemUpdateDto>();
        var updateDelete = new List<PurchaseItemUpdateDto>();

        if (items is null || items.Count == 0)
            return (add, updateDelete);

        foreach (var item in items)
        {
            if (item.OperationType == ItemOperationType.Add)
                add.Add(item);
            else
                updateDelete.Add(item);
        }

        return (add, updateDelete);
    }

    private static int UpdateOrRemoveItems(Purchase purchase, List<PurchaseItemUpdateDto> itemsToUpdateOrDelete)
    {
        var existingMap = purchase.Items.ToDictionary(i => i.Id, i => i);
        int changeCount = 0;

        foreach (var item in itemsToUpdateOrDelete)
        {
            if (item.Id is null || !existingMap.TryGetValue(item.Id.Value, out var existingItem))
                continue;

            switch (item.OperationType)
            {
                case ItemOperationType.Update:
                    var before = (existingItem.ProductId, existingItem.Qty, existingItem.UnitPrice, existingItem.ItemStatus);

                    existingItem.Update(item.ProductId, item.Qty, item.UnitPrice, item.ItemStatus);

                    var after = (existingItem.ProductId, existingItem.Qty, existingItem.UnitPrice, existingItem.ItemStatus);
                    if (before != after)
                    {
                        purchase.QueueDomainEvent(new PurchaseItemUpdated { PurchaseItem = existingItem });
                        changeCount++;
                    }
                    break;

                case ItemOperationType.Remove:
                    purchase.Items.Remove(existingItem);
                    purchase.QueueDomainEvent(new PurchaseItemRemoved { PurchaseItem = existingItem });
                    changeCount++;
                    break;
            }
        }

        return changeCount;
    }
    private async Task<Guid> AddPurchaseWithItemsAsync(UpdatePurchaseCommand request, List<PurchaseItemUpdateDto> itemsToAdd, CancellationToken cancellationToken)
    {
        var newPurchase = Purchase.Create(request.SupplierId, request.PurchaseDate, request.TotalAmount, request.Status);

        foreach (var item in itemsToAdd)
        {
            newPurchase.AddItem(item.ProductId, item.Qty, item.UnitPrice, item.ItemStatus);
        }

        await repository.AddAsync(newPurchase, cancellationToken);
        logger.LogInformation("New purchase created with {ItemCount} items. Id: {PurchaseId}", itemsToAdd.Count, newPurchase.Id);

        return newPurchase.Id;
    }

    private async Task<Guid> UpdatePurchaseItemsAsync(UpdatePurchaseCommand request, List<PurchaseItemUpdateDto> itemsToUpdateOrDelete, CancellationToken cancellationToken)
    {
        var spec = new GetUpdatePurchaseSpecs(request.Id);
        var existingPurchase = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new PurchaseNotFoundException(request.Id);

        existingPurchase.Update(request.SupplierId, request.PurchaseDate, request.TotalAmount, request.Status);

        int changes = UpdateOrRemoveItems(existingPurchase, itemsToUpdateOrDelete);

        await repository.UpdateAsync(existingPurchase, cancellationToken);
        logger.LogInformation("Purchase with Id {PurchaseId} updated. Item changes: {ChangeCount}", existingPurchase.Id, changes);

        return existingPurchase.Id;
    }

}
