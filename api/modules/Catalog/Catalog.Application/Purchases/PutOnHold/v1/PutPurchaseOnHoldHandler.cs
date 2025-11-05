using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.PutOnHold.v1;

public sealed class PutPurchaseOnHoldHandler : IRequestHandler<PutPurchaseOnHoldCommand, PutPurchaseOnHoldResponse>
{
    private readonly IRepository<Purchase> _repository;

    public PutPurchaseOnHoldHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<PutPurchaseOnHoldResponse> Handle(PutPurchaseOnHoldCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.PutOnHold(request.Reason);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new PutPurchaseOnHoldResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase put on hold successfully.",
            request.Reason
        );
    }
}
