using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkShipped.v1;

public class MarkShippedHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<MarkShippedCommand, MarkShippedResponse>
{
    public async Task<MarkShippedResponse> Handle(MarkShippedCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);

        if (purchase == null)
            throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkShipped();
        await repository.SaveChangesAsync(cancellationToken);

        return new MarkShippedResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as shipped successfully."
        );
    }
}
