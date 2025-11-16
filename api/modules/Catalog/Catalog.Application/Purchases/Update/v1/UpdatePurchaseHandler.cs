using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
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
        var purchase = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchase ?? throw new PurchaseNotFoundException(request.Id);

        // Update supplier, date, and remarks
        var updatedPurchase = purchase.Update(request.SupplierId, request.PurchaseDate, null, null, request.DeliveryAddress);

        // Handle status change if provided
        if (request.Status.HasValue && request.Status.Value != purchase.Status)
        {
            switch (request.Status.Value)
            {
                case PurchaseStatus.Submitted:
                    if (purchase.Status == PurchaseStatus.Draft)
                    {
                        updatedPurchase.Submit();
                    }
                    break;
                case PurchaseStatus.Delivered:
                    if (purchase.Status == PurchaseStatus.Submitted || purchase.Status == PurchaseStatus.PartiallyDelivered)
                    {
                        updatedPurchase.MarkAsDelivered();
                    }
                    break;
                case PurchaseStatus.PartiallyDelivered:
                    if (purchase.Status == PurchaseStatus.Submitted)
                    {
                        updatedPurchase.MarkAsPartiallyDelivered();
                    }
                    break;
                case PurchaseStatus.Closed:
                    if (purchase.Status == PurchaseStatus.Delivered)
                    {
                        updatedPurchase.Close();
                    }
                    break;
                case PurchaseStatus.Cancelled:
                    if (purchase.Status != PurchaseStatus.Closed)
                    {
                        updatedPurchase.Cancel();
                    }
                    break;
                // Note: Cannot change to Pending or Draft once progressed
            }
        }

        await repository.UpdateAsync(updatedPurchase, cancellationToken);
        logger.LogInformation("purchase with id : {PurchaseId} updated.", purchase.Id);
        return new UpdatePurchaseResponse(purchase.Id);
    }
}
