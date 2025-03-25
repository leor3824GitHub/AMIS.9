using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
public sealed class DeletePurchaseHandler(
    ILogger<DeletePurchaseHandler> logger,
    [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
    : IRequestHandler<DeletePurchaseCommand>
{
    public async Task Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var purchase = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = purchase ?? throw new PurchaseNotFoundException(request.Id);
        await repository.DeleteAsync(purchase, cancellationToken);
        logger.LogInformation("purchase with id : {PurchaseId} deleted", purchase.Id);
    }
}
