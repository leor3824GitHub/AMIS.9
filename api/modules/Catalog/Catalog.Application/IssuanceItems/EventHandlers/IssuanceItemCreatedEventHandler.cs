using AMIS.WebApi.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.EventHandlers;

public class IssuanceItemCreatedEventHandler(ILogger<IssuanceItemCreatedEventHandler> logger) : INotificationHandler<IssuanceItemCreated>
{
    public async Task Handle(IssuanceItemCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling IssuanceItem created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling IssuanceItem created domain event..");
    }
}

