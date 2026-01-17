using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InspectionRequests.Specifications;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Events;

public sealed class InspectionCreatedHandler : INotificationHandler<InspectionCreated>
{
    private readonly IRepository<InspectionRequest> _repository;
    private readonly ILogger<InspectionCreatedHandler> _logger;

    public InspectionCreatedHandler(
        [FromKeyedServices("inventories:inspectionRequests")] IRepository<InspectionRequest> repository,
        ILogger<InspectionCreatedHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(InspectionCreated notification, CancellationToken cancellationToken)
    {
        // Find inspection request by PurchaseId or InspectorId
        var spec = new GetInspectionRequestByPurchaseSpec(notification.PurchaseId);
        var inspectionRequest = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (inspectionRequest != null && inspectionRequest.Status == InspectionRequestStatus.Assigned)
        {
            // Update status to InProgress when inspection starts
            inspectionRequest.UpdateStatus(InspectionRequestStatus.InProgress);
            await _repository.UpdateAsync(inspectionRequest, cancellationToken);

            _logger.LogInformation(
                "Updated InspectionRequest {RequestId} status to InProgress after Inspection {InspectionId} was created",
                inspectionRequest.Id, notification.InspectionId);
        }
    }
}
