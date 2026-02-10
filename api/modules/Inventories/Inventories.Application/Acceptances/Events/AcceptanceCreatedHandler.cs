using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.WebApi.Inventories.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Events;

public sealed class AcceptanceCreatedHandler : INotificationHandler<AcceptanceCreated>
{
    private readonly ILogger<AcceptanceCreatedHandler> _logger;

    public AcceptanceCreatedHandler(ILogger<AcceptanceCreatedHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(AcceptanceCreated notification, CancellationToken cancellationToken)
    {
        if (notification.Acceptance is null)
        {
            _logger.LogWarning("Received AcceptanceCreated event without an acceptance payload.");
            return;
        }

        var acceptance = notification.Acceptance;

        _logger.LogInformation("Acceptance {AcceptanceId} created for purchase {PurchaseId}.", acceptance.Id, acceptance.PurchaseId);

        // TODO: Add any post-acceptance operations here (e.g., inventory updates, notifications)
        await Task.CompletedTask;
    }
}

