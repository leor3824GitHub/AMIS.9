using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPendingPayment.v1;

public sealed class MarkPendingPaymentHandler : IRequestHandler<MarkPendingPaymentCommand, MarkPendingPaymentResponse>
{
    private readonly IRepository<Purchase> _repository;

    public MarkPendingPaymentHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<MarkPendingPaymentResponse> Handle(MarkPendingPaymentCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkPendingPayment();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkPendingPaymentResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as pending payment successfully."
        );
    }
}
