using AMIS.WebApi.Inventories.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Employees.EventHandlers;

public class EmployeeCreatedEventHandler(ILogger<EmployeeCreatedEventHandler> logger) : INotificationHandler<EmployeeCreated>
{
    public async Task Handle(EmployeeCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling employee created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling employee created domain event..");
    }
}

