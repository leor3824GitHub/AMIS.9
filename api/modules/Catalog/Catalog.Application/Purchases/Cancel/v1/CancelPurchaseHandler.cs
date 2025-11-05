using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.Cancel.v1;

public class CancelPurchaseHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<CancelPurchaseCommand, CancelPurchaseResponse>
{
    public async Task<CancelPurchaseResponse> Handle(CancelPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);

        if (purchase == null)
            throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.Cancel(request.Reason);
        await repository.SaveChangesAsync(cancellationToken);

        return new CancelPurchaseResponse(
            purchase.Id,
            purchase.Status!.Value,
            request.Reason,
            "Purchase cancelled successfully."
        );
    }
}
