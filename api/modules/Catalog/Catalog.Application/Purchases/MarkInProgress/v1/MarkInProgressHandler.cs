using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkInProgress.v1;

public sealed class MarkInProgressHandler : IRequestHandler<MarkInProgressCommand, MarkInProgressResponse>
{
    private readonly IRepository<Purchase> _repository;

    public MarkInProgressHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<MarkInProgressResponse> Handle(MarkInProgressCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkInProgress();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkInProgressResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as in progress successfully."
        );
    }
}
