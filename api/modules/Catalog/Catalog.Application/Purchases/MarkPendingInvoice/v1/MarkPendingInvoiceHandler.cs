using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.MarkPendingInvoice.v1;

public sealed class MarkPendingInvoiceHandler : IRequestHandler<MarkPendingInvoiceCommand, MarkPendingInvoiceResponse>
{
    private readonly IRepository<Purchase> _repository;

    public MarkPendingInvoiceHandler([FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<MarkPendingInvoiceResponse> Handle(MarkPendingInvoiceCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _repository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new InvalidOperationException($"Purchase with ID {request.PurchaseId} not found.");

        purchase.MarkPendingInvoice();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkPendingInvoiceResponse(
            purchase.Id,
            purchase.Status!.Value,
            "Purchase marked as pending invoice successfully."
        );
    }
}
