using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkFullyReceived.v1;

public class MarkFullyReceivedHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<MarkFullyReceivedCommand, MarkFullyReceivedResponse>
{
    public async Task<MarkFullyReceivedResponse> Handle(MarkFullyReceivedCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);

        if (purchase == null)
            throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkFullyReceived();
        await repository.SaveChangesAsync(cancellationToken);

        return new MarkFullyReceivedResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as fully received successfully."
        );
    }
}
