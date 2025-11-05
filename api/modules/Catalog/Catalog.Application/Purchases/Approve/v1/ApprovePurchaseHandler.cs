using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.Approve.v1;

internal sealed class ApprovePurchaseHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<ApprovePurchaseCommand, ApprovePurchaseResponse>
{
    public async Task<ApprovePurchaseResponse> Handle(ApprovePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.Approve();

        await repository.SaveChangesAsync(cancellationToken);

        return new ApprovePurchaseResponse(
            purchase.Id,
            purchase.Status?.ToString() ?? "Unknown",
            "Purchase approved successfully.");
    }
}
