using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.SubmitForApproval.v1;

internal sealed class SubmitPurchaseForApprovalHandler(
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<SubmitPurchaseForApprovalCommand, SubmitPurchaseForApprovalResponse>
{
    public async Task<SubmitPurchaseForApprovalResponse> Handle(SubmitPurchaseForApprovalCommand request, CancellationToken cancellationToken)
    {
        var purchase = await repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.SubmitForApproval();

        await repository.SaveChangesAsync(cancellationToken);

        return new SubmitPurchaseForApprovalResponse(
            purchase.Id,
            purchase.Status?.ToString() ?? "Unknown",
            "Purchase submitted for approval successfully.");
    }
}
