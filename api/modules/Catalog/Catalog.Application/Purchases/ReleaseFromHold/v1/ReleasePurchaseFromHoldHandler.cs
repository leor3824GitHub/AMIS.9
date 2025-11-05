using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.ReleaseFromHold.v1;

public sealed class ReleasePurchaseFromHoldHandler : IRequestHandler<ReleasePurchaseFromHoldCommand, ReleasePurchaseFromHoldResponse>
{
    private readonly IRepository<Purchase> _repository;

    public ReleasePurchaseFromHoldHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<ReleasePurchaseFromHoldResponse> Handle(ReleasePurchaseFromHoldCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.ReleaseFromHold();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new ReleasePurchaseFromHoldResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase released from hold successfully."
        );
    }
}
