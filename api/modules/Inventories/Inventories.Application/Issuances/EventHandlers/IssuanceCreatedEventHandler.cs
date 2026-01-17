using AMIS.WebApi.Inventories.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Issuances.EventHandlers;

public class IssuanceCreatedEventHandler(ILogger<IssuanceCreatedEventHandler> logger) : INotificationHandler<IssuanceCreated>
{
    public async Task Handle(IssuanceCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling issuance created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling issuance created domain event..");
    }
}


