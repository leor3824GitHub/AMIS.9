using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.Inspections.Events;

/// <summary>
/// Handles InspectionApproved event notifications.
/// Logs the approval event and executes post-approval operations.
/// </summary>
public sealed class InspectionApprovedHandler : INotificationHandler<InspectionApproved>
{
    private readonly IReadRepository<Inspection> _inspectionReadRepo;
    private readonly IRepository<Purchase> _purchaseRepo;
    private readonly ILogger<InspectionApprovedHandler> _logger;

    public InspectionApprovedHandler(
        [FromKeyedServices("inventories:inspections")] IReadRepository<Inspection> inspectionReadRepo,
        [FromKeyedServices("inventories:purchases")] IRepository<Purchase> purchaseRepo,
        ILogger<InspectionApprovedHandler> logger)
    {
        _inspectionReadRepo = inspectionReadRepo ?? throw new ArgumentNullException(nameof(inspectionReadRepo));
        _purchaseRepo = purchaseRepo ?? throw new ArgumentNullException(nameof(purchaseRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InspectionApproved notification, CancellationToken cancellationToken)
    {
        if (notification is null)
        {
            _logger.LogWarning("Received null InspectionApproved notification.");
            return;
        }

        var inspection = await _inspectionReadRepo.GetByIdAsync(notification.InspectionId, cancellationToken);
        if (inspection is null)
        {
            _logger.LogWarning("Inspection {InspectionId} not found.", notification.InspectionId);
            return;
        }

        _logger.LogInformation("Inspection {InspectionId} approved by {EmployeeId} on {ApprovedOn}.",
            notification.InspectionId, notification.EmployeeId, notification.ApprovedOn);

        // TODO: Add post-approval operations here based on inspection type
        // Examples:
        // - For NewDelivery: Create acceptance record, update purchase status
        // - For AssetReturn/Repair: Update asset inventory status, trigger accounting entries
        // - Send notifications to relevant stakeholders

        await Task.CompletedTask;
    }
}

