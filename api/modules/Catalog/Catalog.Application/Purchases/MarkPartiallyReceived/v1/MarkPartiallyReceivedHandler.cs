using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPartiallyReceived.v1;

public sealed class MarkPartiallyReceivedHandler : IRequestHandler<MarkPartiallyReceivedCommand, MarkPartiallyReceivedResponse>
{
    private readonly IRepository<Purchase> _repository;

    public MarkPartiallyReceivedHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<MarkPartiallyReceivedResponse> Handle(MarkPartiallyReceivedCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkPartiallyReceived();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkPartiallyReceivedResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as partially received successfully."
        );
    }
}
