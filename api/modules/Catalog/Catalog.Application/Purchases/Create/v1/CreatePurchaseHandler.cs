using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1
{
    public sealed class CreatePurchaseHandler(
        ILogger<CreatePurchaseHandler> logger,
        [FromKeyedServices("catalog:purchases")] IRepository<Purchase> repository)
        : IRequestHandler<CreatePurchaseCommand, CreatePurchaseResponse>
    {
        public async Task<CreatePurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Create the purchase entity (aggregate root)
            var purchase = Purchase.Create(
                request.SupplierId,
                request.PurchaseDate,
                request.TotalAmount,
                request.Status
            );

            // Only add items if provided in the request
            if (request.Items is { Count: > 0 })
            {
                foreach (var item in request.Items)
                {
                    // Create and add new PurchaseItems
                    var newItem = PurchaseItem.Create(
                        purchase.Id,
                        item.ProductId,
                        item.Qty,
                        item.UnitPrice,
                        item.ItemStatus
                    );

                    purchase.Items.Add(newItem);  // Add to the purchase

                    // Queue domain event for the new item creation
                    purchase.QueueDomainEvent(new PurchaseItemCreated { PurchaseItem = newItem });
                }
            }

            // Recalculate the total amount of the purchase
            var total = purchase.Items.Sum(i => i.Qty * i.UnitPrice);
            purchase.Update(purchase.SupplierId, purchase.PurchaseDate, total, purchase.Status);

            // Persist the purchase entity and its items to the repository
            await repository.AddAsync(purchase, cancellationToken);

            // Prepare the response DTO for the purchase items
            var itemDtos = purchase.Items.Select(i =>
                new PurchaseItemDto(i.Id, i.ProductId, i.Qty, i.UnitPrice, i.ItemStatus)).ToList();

            // Log the creation of the purchase
            logger.LogInformation("Purchase created with ID {PurchaseId}", purchase.Id);

            // Return the response with the created purchase ID and items
            return new CreatePurchaseResponse(purchase.Id, itemDtos);
        }
    }
}
