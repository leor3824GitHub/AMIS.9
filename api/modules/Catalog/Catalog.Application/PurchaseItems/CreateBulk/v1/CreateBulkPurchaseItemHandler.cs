using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.CreateBulk.v1;

public sealed class CreateBulkPurchaseItemHandler(
    ILogger<CreateBulkPurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository)
    : IRequestHandler<CreateBulkPurchaseItemCommand, CreateBulkPurchaseItemResponse>
{
    public async Task<CreateBulkPurchaseItemResponse> Handle(CreateBulkPurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Items == null || request.Items.Count == 0)
        {
            throw new ArgumentException("No purchase items provided in request.");
        }

        // Create purchase item entities from DTOs
        var purchaseItems = request.Items.Select(item =>
            PurchaseItem.Create(
                request.PurchaseId,
                item.ProductId!,
                item.Qty,
                item.UnitPrice,
                item.ItemStatus
            )).ToList();

        // Bulk insert
        var resultItems = await repository.AddRangeAsync(purchaseItems, cancellationToken);
        logger.LogInformation("Bulk created {Count} purchase items for PurchaseId {PurchaseId}", purchaseItems.Count, request.PurchaseId);

        // Return the IDs of the created items
        return new CreateBulkPurchaseItemResponse(resultItems.Select(x => x.Id).ToList());
    }
}
