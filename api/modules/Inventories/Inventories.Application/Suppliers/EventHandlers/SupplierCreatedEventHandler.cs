using AMIS.WebApi.Inventories.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Suppliers.EventHandlers;

public class SupplierCreatedEventHandler(ILogger<SupplierCreatedEventHandler> logger) : INotificationHandler<SupplierCreated>
{
    public async Task Handle(SupplierCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling supplier created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling supplier created domain event..");
    }
}

