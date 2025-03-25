using AMIS.WebApi.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Purchases.EventHandlers;

public class PurchaseCreatedEventHandler(ILogger<PurchaseCreatedEventHandler> logger) : INotificationHandler<PurchaseCreated>
{
    public async Task Handle(PurchaseCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling purchase created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling purchase created domain event..");
    }
}

