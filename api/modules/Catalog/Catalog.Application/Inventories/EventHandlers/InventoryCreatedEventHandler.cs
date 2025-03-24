using AMIS.WebApi.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Inventories.EventHandlers;

public class InventoryCreatedEventHandler(ILogger<InventoryCreatedEventHandler> logger) : INotificationHandler<InventoryCreated>
{
    public async Task Handle(InventoryCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling inventory created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling inventory created domain event..");
    }
}

