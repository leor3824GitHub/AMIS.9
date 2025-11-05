using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkClosed.v1;

public sealed class MarkClosedHandler : IRequestHandler<MarkClosedCommand, MarkClosedResponse>
{
    private readonly IRepository<Purchase> _repository;

    public MarkClosedHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<MarkClosedResponse> Handle(MarkClosedCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkClosed();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkClosedResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as closed successfully."
        );
    }
}
