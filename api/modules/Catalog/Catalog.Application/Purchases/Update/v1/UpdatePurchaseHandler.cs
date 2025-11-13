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
        var purchase = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchase ?? throw new PurchaseNotFoundException(request.Id);

        // Update only supports changing supplier, date, and remarks - not totalAmount or status
        // TotalAmount is calculated automatically, Status has dedicated methods
        var updatedPurchase = purchase.Update(request.SupplierId, request.PurchaseDate, null, null);
        await repository.UpdateAsync(updatedPurchase, cancellationToken);
        logger.LogInformation("purchase with id : {PurchaseId} updated.", purchase.Id);
        return new UpdatePurchaseResponse(purchase.Id);
    }
}
