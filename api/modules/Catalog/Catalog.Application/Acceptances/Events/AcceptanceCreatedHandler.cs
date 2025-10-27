using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Events;

public sealed class AcceptanceCreatedHandler : INotificationHandler<AcceptanceCreated>
{
    private readonly IRepository<InspectionRequest> _inspectionRequestRepository;
    private readonly ILogger<AcceptanceCreatedHandler> _logger;

    public AcceptanceCreatedHandler(
        [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> inspectionRequestRepository,
        ILogger<AcceptanceCreatedHandler> logger)
    {
        _inspectionRequestRepository = inspectionRequestRepository ?? throw new ArgumentNullException(nameof(inspectionRequestRepository));
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
        if (acceptance.PurchaseId == Guid.Empty)
        {
            _logger.LogDebug("Acceptance {AcceptanceId} has no PurchaseId. Skipping inspection request status update.", acceptance.Id);
            return;
        }

        var spec = new GetInspectionRequestByPurchaseSpec(acceptance.PurchaseId);
        var inspectionRequest = await _inspectionRequestRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inspectionRequest is null)
        {
            _logger.LogDebug("No inspection request found for purchase {PurchaseId} when handling acceptance {AcceptanceId}.", acceptance.PurchaseId, acceptance.Id);
            return;
        }

        if (inspectionRequest.Status == InspectionRequestStatus.Accepted)
        {
            _logger.LogTrace("Inspection request {RequestId} is already Accepted.", inspectionRequest.Id);
            return;
        }

        inspectionRequest.MarkAccepted();
        await _inspectionRequestRepository.UpdateAsync(inspectionRequest, cancellationToken);

        _logger.LogInformation("Inspection request {RequestId} marked as Accepted after acceptance {AcceptanceId}.", inspectionRequest.Id, acceptance.Id);
    }
}
