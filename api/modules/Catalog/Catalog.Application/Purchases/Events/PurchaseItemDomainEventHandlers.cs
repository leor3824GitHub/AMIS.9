using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Purchases.Events;

/// <summary>
/// Handles inspection progress updates for a purchase when a PurchaseItem's inspection status changes.
/// Automatically transitions Purchase status:
/// Submitted -> PartiallyDelivered (first item inspected)
/// Submitted/PartiallyDelivered -> Delivered (all items fully inspected)
/// </summary>
public sealed class PurchaseItemInspectedHandler : INotificationHandler<PurchaseItemInspected>
{
    private readonly IRepository<Purchase> _purchaseRepo;
    private readonly ILogger<PurchaseItemInspectedHandler> _logger;

    public PurchaseItemInspectedHandler(
        [FromKeyedServices("catalog:purchases")] IRepository<Purchase> purchaseRepo,
        ILogger<PurchaseItemInspectedHandler> logger)
    {
        _purchaseRepo = purchaseRepo ?? throw new ArgumentNullException(nameof(purchaseRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(PurchaseItemInspected notification, CancellationToken cancellationToken)
    {
        var item = notification.PurchaseItem;
        if (item is null)
        {
            _logger.LogWarning("PurchaseItemInspected event received with null item");
            return;
        }

        var purchase = await _purchaseRepo.GetByIdAsync(item.PurchaseId, cancellationToken);
        if (purchase is null)
        {
            _logger.LogWarning("Purchase {PurchaseId} not found for inspected item {ItemId}", item.PurchaseId, item.Id);
            return;
        }

        bool anyInspected = purchase.Items.Any(i => i.InspectionStatus != PurchaseItemInspectionStatus.NotInspected);
        bool fullyInspected = purchase.Items.Count > 0 && purchase.Items.All(i => 
            i.InspectionStatus == PurchaseItemInspectionStatus.Passed || 
            i.InspectionStatus == PurchaseItemInspectionStatus.Failed);

        try
        {
            if (purchase.Status == PurchaseStatus.Submitted && anyInspected && !fullyInspected)
            {
                purchase.MarkAsPartiallyDelivered();
                _logger.LogInformation("Purchase {PurchaseId} auto-transitioned to PartiallyDelivered after inspection progress", purchase.Id);
            }
            else if ((purchase.Status == PurchaseStatus.Submitted || purchase.Status == PurchaseStatus.PartiallyDelivered) && fullyInspected)
            {
                purchase.MarkAsDelivered();
                _logger.LogInformation("Purchase {PurchaseId} auto-transitioned to Delivered (all items inspected)", purchase.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Auto status transition skipped for Purchase {PurchaseId}", purchase.Id);
        }

        await _purchaseRepo.UpdateAsync(purchase, cancellationToken);
    }
}

/// <summary>
/// Handles acceptance progress updates for a purchase when a PurchaseItem's acceptance status changes.
/// Automatically closes Purchase when Delivered + fully inspected + fully accepted.
/// </summary>
public sealed class PurchaseItemAcceptedHandler : INotificationHandler<PurchaseItemAccepted>
{
    private readonly IRepository<Purchase> _purchaseRepo;
    private readonly ILogger<PurchaseItemAcceptedHandler> _logger;

    public PurchaseItemAcceptedHandler(
        [FromKeyedServices("catalog:purchases")] IRepository<Purchase> purchaseRepo,
        ILogger<PurchaseItemAcceptedHandler> logger)
    {
        _purchaseRepo = purchaseRepo ?? throw new ArgumentNullException(nameof(purchaseRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(PurchaseItemAccepted notification, CancellationToken cancellationToken)
    {
        var item = notification.PurchaseItem;
        if (item is null)
        {
            _logger.LogWarning("PurchaseItemAccepted event received with null item");
            return;
        }

        var purchase = await _purchaseRepo.GetByIdAsync(item.PurchaseId, cancellationToken);
        if (purchase is null)
        {
            _logger.LogWarning("Purchase {PurchaseId} not found for accepted item {ItemId}", item.PurchaseId, item.Id);
            return;
        }

        bool fullyAccepted = purchase.Items.Count > 0 && purchase.Items.All(i => i.AcceptanceStatus == PurchaseItemAcceptanceStatus.Accepted);
        bool fullyInspected = purchase.Items.Count > 0 && purchase.Items.All(i => 
            i.InspectionStatus == PurchaseItemInspectionStatus.Passed || 
            i.InspectionStatus == PurchaseItemInspectionStatus.Failed);

        // Only attempt auto-close if purchase already marked Delivered
        if (purchase.Status == PurchaseStatus.Delivered && fullyInspected && fullyAccepted)
        {
            try
            {
                purchase.Close();
                _logger.LogInformation("Purchase {PurchaseId} auto-closed (fully inspected & accepted)", purchase.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Auto close failed for Purchase {PurchaseId}", purchase.Id);
            }
        }

        await _purchaseRepo.UpdateAsync(purchase, cancellationToken);
    }
}
