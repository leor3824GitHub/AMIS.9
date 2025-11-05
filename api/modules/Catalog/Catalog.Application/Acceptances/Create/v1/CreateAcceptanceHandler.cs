using System;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;
using AMIS.WebApi.Catalog.Domain;
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
    [FromKeyedServices("catalog:purchaseItems")] IReadRepository<PurchaseItem> purchaseItemReadRepository)
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
            var inspectionRequest = await inspectionRequestRepository.GetByIdAsync(insp.InspectionRequestId, cancellationToken);

            if (inspectionRequest?.PurchaseId == null)
            {
                throw new InvalidOperationException("The specified inspection is not linked to a purchase; cannot create acceptance.");
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
            throw new InvalidOperationException("Submit an inspection request before recording an acceptance.");
        }

        if (inspectionRequest2.Status is not InspectionRequestStatus.Completed and not InspectionRequestStatus.Accepted)
        {
            throw new InvalidOperationException("Complete the inspection before recording an acceptance.");
        }

        Guid? inspectionId = effectiveInspectionId;
        if (!inspectionId.HasValue)
        {
            var inspectionSpec = new GetLatestInspectionByPurchaseSpec(effectivePurchaseId);
            var inspection = await inspectionRepository.FirstOrDefaultAsync(inspectionSpec, cancellationToken);

            if (inspection is null)
            {
                throw new InvalidOperationException("Record an inspection for the purchase before creating an acceptance.");
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
            foreach (var item in request.Items)
            {
                // Load purchase item with existing posted acceptances and inspection summaries
                var piSpec = new AMIS.WebApi.Catalog.Application.PurchaseItems.Specifications.GetPurchaseItemWithAcceptancesSpec(item.PurchaseItemId);
                var purchaseItem = await purchaseItemReadRepository.FirstOrDefaultAsync(piSpec, cancellationToken)
                    ?? throw new InvalidOperationException($"Purchase item {item.PurchaseItemId} not found.");

                // Single-shot guard: reject if any acceptance already exists for this purchase item
                if (purchaseItem.AcceptanceItems.Count > 0)
                {
                    throw new InvalidOperationException($"An acceptance has already been recorded for purchase item {item.PurchaseItemId}. Single-shot acceptance is enforced.");
                }

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
