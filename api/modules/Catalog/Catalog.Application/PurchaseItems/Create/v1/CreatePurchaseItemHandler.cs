using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v1;
public sealed class CreatePurchaseItemHandler(
    ILogger<CreatePurchaseItemHandler> logger,
    [FromKeyedServices("catalog:purchaseItems")] IRepository<PurchaseItem> repository)
    : IRequestHandler<CreatePurchaseItemCommand, CreatePurchaseItemResponse>
{
    public async Task<CreatePurchaseItemResponse> Handle(CreatePurchaseItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var purchaseItem = PurchaseItem.Create(request.PurchaseId!, request.ProductId!, request.Qty, request.UnitPrice, request.Status);
        await repository.AddAsync(purchaseItem, cancellationToken);
        logger.LogInformation("purchaseItem created {PurchaseItemId}", purchaseItem.Id);
        return new CreatePurchaseItemResponse(purchaseItem.Id);
    }
}
