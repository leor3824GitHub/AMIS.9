using AMIS.WebApi.Inventories.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PurchaseItems.EventHandlers;

public class PurchaseItemCreatedEventHandler(ILogger<PurchaseItemCreatedEventHandler> logger) : INotificationHandler<PurchaseItemCreated>
{
    public async Task Handle(PurchaseItemCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling PurchaseItem created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling PurchaseItem created domain event..");
    }
}


