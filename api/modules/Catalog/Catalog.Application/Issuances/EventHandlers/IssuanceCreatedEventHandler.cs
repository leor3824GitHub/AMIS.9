using AMIS.WebApi.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Issuances.EventHandlers;

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

