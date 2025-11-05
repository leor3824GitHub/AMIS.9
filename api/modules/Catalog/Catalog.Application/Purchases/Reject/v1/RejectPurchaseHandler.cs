using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.Reject.v1;

internal sealed class RejectPurchaseHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<RejectPurchaseCommand, RejectPurchaseResponse>
{
    public async Task<RejectPurchaseResponse> Handle(RejectPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.Reject(request.Reason);

        await repository.SaveChangesAsync(cancellationToken);

        return new RejectPurchaseResponse(
            purchase.Id,
            purchase.Status?.ToString() ?? "Unknown",
            $"Purchase rejected: {request.Reason}");
    }
}
