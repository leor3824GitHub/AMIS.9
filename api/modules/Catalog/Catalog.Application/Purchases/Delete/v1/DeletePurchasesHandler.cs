using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AMIS.WebApi.Catalog.Application.Purchases.Delete.v1
{
    public sealed class DeletePurchasesHandler(
        ILogger<DeletePurchaseHandler> logger,
        [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
        : IRequestHandler<DeletePurchasesCommand>
    {       
        public async Task Handle(DeletePurchasesCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var purchases = new List<Purchase>();

            foreach (var purchaseId in request.PurchaseIds)
            {
                var purchase = await repository.GetByIdAsync(purchaseId, cancellationToken);
                if (purchase != null)
                {
                    purchases.Add(purchase);
                }
            }

            if (purchases.Count == 0)
            {
                logger.LogInformation("No purchases found for the provided {PurchaseCount} IDs", purchases.Count);
                //throw new PurchaseNotFoundException("No purchases found for the provided IDs.");
            }

            await repository.DeleteRangeAsync(purchases, cancellationToken);
            logger.LogInformation("{PurchaseCount} purchases deleted", purchases.Count);
        }
    }
}
