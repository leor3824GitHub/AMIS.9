using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkInvoiced.v1;

public class MarkInvoicedHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<MarkInvoicedCommand, MarkInvoicedResponse>
{
    public async Task<MarkInvoicedResponse> Handle(MarkInvoicedCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken);

        if (purchase == null)
            throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkInvoiced();
        await repository.SaveChangesAsync(cancellationToken);

        return new MarkInvoicedResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as invoiced successfully."
        );
    }
}
