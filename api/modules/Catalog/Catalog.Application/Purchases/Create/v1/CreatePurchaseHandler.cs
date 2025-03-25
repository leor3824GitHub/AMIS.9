using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public sealed class CreatePurchaseHandler(
    ILogger<CreatePurchaseHandler> logger,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<CreatePurchaseCommand, CreatePurchaseResponse>
{
    public async Task<CreatePurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var purchase = Purchase.Create(request.SupplierId!, request.PurchaseDate, request.TotalAmount, request.Status);
        await repository.AddAsync(purchase, cancellationToken);
        logger.LogInformation("purchase created {PurchaseId}", purchase.Id);
        return new CreatePurchaseResponse(purchase.Id);
    }
}
