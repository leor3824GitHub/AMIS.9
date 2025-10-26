//csharp api/modules/Catalog/Catalog.Application/Inspections/Events/InspectionApprovedHandler.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection; // For FromKeyedServices

namespace AMIS.WebApi.Catalog.Application.Inspections.Events;

public sealed class InspectionApprovedHandler : INotificationHandler<InspectionApproved>
{
    private readonly IReadRepository<Inspection> _inspectionReadRepo;
    private readonly IRepository<InspectionRequest> _inspectionRequestRepo;
    private readonly ILogger<InspectionApprovedHandler> _logger;

    public InspectionApprovedHandler(
        [FromKeyedServices("catalog:inspections")] IReadRepository<Inspection> inspectionReadRepo,
        [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> inspectionRequestRepo,
        ILogger<InspectionApprovedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
        _inspectionRequestRepo = inspectionRequestRepo ?? throw new ArgumentNullException(nameof(inspectionRequestRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InspectionApproved notification, CancellationToken cancellationToken)
    {
        var spec = new GetInspectionWithItemsSpec(notification.InspectionId);
        var inspection = await _inspectionReadRepo.FirstOrDefaultAsync(spec, cancellationToken);
        if (inspection == null)
        {
            _logger.LogWarning("Inspection {InspectionId} not found when handling InspectionApproved.", notification.InspectionId);
            return;
        }

        if (!notification.PurchaseId.HasValue)
        {
            return;
        }

        var requestSpec = new AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications.GetInspectionRequestByPurchaseSpec(notification.PurchaseId.Value);
        var inspectionRequest = await _inspectionRequestRepo.FirstOrDefaultAsync(requestSpec, cancellationToken);

        if (inspectionRequest != null && inspectionRequest.Status != AMIS.WebApi.Catalog.Domain.ValueObjects.InspectionRequestStatus.Completed)
        {
            inspectionRequest.MarkCompleted();
            await _inspectionRequestRepo.UpdateAsync(inspectionRequest, cancellationToken);
            _logger.LogInformation("Marked InspectionRequest {RequestId} as Completed after Inspection {InspectionId} was approved",
                inspectionRequest.Id, notification.InspectionId);
        }
    }
}
