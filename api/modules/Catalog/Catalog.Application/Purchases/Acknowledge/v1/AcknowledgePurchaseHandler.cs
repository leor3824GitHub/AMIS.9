using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.Acknowledge.v1;

public sealed class AcknowledgePurchaseHandler : IRequestHandler<AcknowledgePurchaseCommand, AcknowledgePurchaseResponse>
{
    private readonly IRepository<Purchase> _repository;

    public AcknowledgePurchaseHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<AcknowledgePurchaseResponse> Handle(AcknowledgePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.Acknowledge();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new AcknowledgePurchaseResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase acknowledged successfully."
        );
    }
}
