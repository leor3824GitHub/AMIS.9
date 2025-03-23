using AMIS.WebApi.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.CategorIEs.EventHandlers;

public class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger) : INotificationHandler<CategoryCreated>
{
    public async Task Handle(CategoryCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling category created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling category created domain event..");
    }
}
