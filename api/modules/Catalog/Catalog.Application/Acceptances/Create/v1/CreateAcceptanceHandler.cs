using System;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Create.v1;

public sealed class CreateAcceptanceHandler(
    ILogger<CreateAcceptanceHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> repository,
    [FromKeyedServices("catalog:inspectionRequests")] IReadRepository<InspectionRequest> inspectionRequestRepository,
    [FromKeyedServices("catalog:inspections")] IReadRepository<Inspection> inspectionRepository,
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> purchaseReadRepository)
    : IRequestHandler<CreateAcceptanceCommand, CreateAcceptanceResponse>
{
    public async Task<CreateAcceptanceResponse> Handle(CreateAcceptanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Prefer linking acceptance by InspectionId when provided. Derive PurchaseId from the inspection to keep data consistent.
        Guid effectivePurchaseId;
        Guid? effectiveInspectionId = request.InspectionId;

        if (effectiveInspectionId.HasValue)
        {
            var insp = await inspectionRepository.GetByIdAsync(effectiveInspectionId.Value, cancellationToken)
                      ?? throw new InvalidOperationException("Specified inspection was not found.");

            // Access PurchaseId through InspectionRequest (following aggregate boundaries)
            InspectionRequest? inspectionRequest = null;
            if (insp.InspectionRequestId.HasValue)
            {
                inspectionRequest = await inspectionRequestRepository.GetByIdAsync(insp.InspectionRequestId.Value, cancellationToken);
            }

            if (inspectionRequest?.PurchaseId == null)
            {
                throw AcceptanceValidationException.ForInspectionNotLinkedToPurchase();
            }

            effectivePurchaseId = inspectionRequest.PurchaseId.Value;
        }
        else
        {
            // Backward-compatibility: fall back to purchase-based flow
            effectivePurchaseId = request.PurchaseId;
        }

        var inspectionRequestSpec = new GetInspectionRequestByPurchaseSpec(effectivePurchaseId);
        var inspectionRequest2 = await inspectionRequestRepository.FirstOrDefaultAsync(inspectionRequestSpec, cancellationToken);

        if (inspectionRequest2 is null)
        {
            throw AcceptanceValidationException.ForMissingInspectionRequest();
        }

        if (inspectionRequest2.Status is not InspectionRequestStatus.Completed and not InspectionRequestStatus.Accepted)
        {
            throw AcceptanceValidationException.ForInspectionNotCompleted();
        }

        Guid? inspectionId = effectiveInspectionId;
        if (!inspectionId.HasValue)
        {
            var inspectionSpec = new GetLatestInspectionByPurchaseSpec(effectivePurchaseId);
            var inspection = await inspectionRepository.FirstOrDefaultAsync(inspectionSpec, cancellationToken);

            if (inspection is null)
            {
                throw AcceptanceValidationException.ForMissingInspection();
            }

            inspectionId = inspection.Id;
        }

        var acceptance = Acceptance.Create(
            purchaseId: effectivePurchaseId,
            supplyOfficerId: request.SupplyOfficerId,
            acceptanceDate: request.AcceptanceDate,
            remarks: request.Remarks,
            inspectionId: inspectionId
        );

        if (request.Items is not null)
        {
            // Load the purchase with its items and any prior acceptance items to validate single-shot and quantity rules
            var purchaseSpec = new AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1.GetPurchaseWithItemsSpecs(effectivePurchaseId);
            var purchase = await purchaseReadRepository.FirstOrDefaultAsync(purchaseSpec, cancellationToken)
                          ?? throw new InvalidOperationException($"Purchase {effectivePurchaseId} not found.");

            foreach (var item in request.Items)
            {
                var purchaseItem = purchase.Items.FirstOrDefault(pi => pi.Id == item.PurchaseItemId)
                    ?? throw new InvalidOperationException($"Purchase item {item.PurchaseItemId} not found in purchase {effectivePurchaseId}.");

                // Single-shot guard: reject if any acceptance already exists for this purchase item
                if (purchaseItem.AcceptanceItems != null && purchaseItem.AcceptanceItems.Count > 0)
                    throw new InvalidOperationException($"An acceptance has already been recorded for purchase item {item.PurchaseItemId}. Single-shot acceptance is enforced.");

                // Ordered-qty guard: do not allow accepting more than ordered
                if (item.QtyAccepted > purchaseItem.Qty)
                {
                    throw new InvalidOperationException($"Accepted quantity {item.QtyAccepted} exceeds ordered quantity {purchaseItem.Qty} for purchase item {item.PurchaseItemId}.");
                }

                if (item.QtyAccepted <= 0)
                {
                    throw new InvalidOperationException("Accepted quantity must be greater than zero.");
                }

                acceptance.AddItem(item.PurchaseItemId, item.QtyAccepted, item.Remarks);
            }
        }

        if (request.PostToInventory && acceptance.Items.Count > 0)
        {
            acceptance.PostAcceptance();
        }

        await repository.AddAsync(acceptance, cancellationToken);
        logger.LogInformation("Acceptance created {AcceptanceId}", acceptance.Id);

        return new CreateAcceptanceResponse(acceptance.Id);
    }
}
