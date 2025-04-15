using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
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
        var spec = new GetUpdatePurchaseSpecs(request.Id);
        var purchase = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (purchase is null)
        {
            throw new PurchaseNotFoundException(request.Id);
        }

        // Update base purchase fields
        purchase.Update(request.SupplierId, request.PurchaseDate, request.TotalAmount, request.Status);

        // Update items using domain method
        var itemUpdates = request.Items.Select(i => new PurchaseItemUpdate(
            i.Id,
            i.ProductId,
            i.Qty,
            i.UnitPrice,
            i.ItemStatus
        )).ToList();

        purchase.SyncItems(itemUpdates);

        // Save updated aggregate root
        await repository.UpdateAsync(purchase, cancellationToken);

        logger.LogInformation("Purchase with ID {PurchaseId} successfully updated.", purchase.Id);

        return new UpdatePurchaseResponse(purchase.Id);
    }
}
