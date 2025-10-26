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

        var inspectionRequestSpec = new GetInspectionRequestByPurchaseSpec(request.PurchaseId);
        var inspectionRequest = await inspectionRequestRepository.FirstOrDefaultAsync(inspectionRequestSpec, cancellationToken);

        if (inspectionRequest is null)
        {
            throw new InvalidOperationException("Submit an inspection request before recording an acceptance.");
        }

        if (inspectionRequest.Status is not InspectionRequestStatus.Completed and not InspectionRequestStatus.Accepted)
        {
            throw new InvalidOperationException("Complete the inspection before recording an acceptance.");
        }

        Guid? inspectionId = request.InspectionId;
        if (!inspectionId.HasValue)
        {
            var inspectionSpec = new GetLatestInspectionByPurchaseSpec(request.PurchaseId);
            var inspection = await inspectionRepository.FirstOrDefaultAsync(inspectionSpec, cancellationToken);

            if (inspection is null)
            {
                throw new InvalidOperationException("Record an inspection for the purchase before creating an acceptance.");
            }

            inspectionId = inspection.Id;
        }

        var acceptance = Acceptance.Create(
            purchaseId: request.PurchaseId,
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

                var approved = purchaseItem.QtyPassed ?? purchaseItem.Qty; // cap to inspected passed qty when available
                if (approved <= 0)
                {
                    throw new InvalidOperationException("Cannot accept items with zero approved quantity. Complete inspection first.");
                }

                // Single-shot guard: reject if any acceptance already exists for this purchase item
                if (purchaseItem.AcceptanceItems.Count > 0)
                {
                    throw new InvalidOperationException($"An acceptance has already been recorded for purchase item {item.PurchaseItemId}. Single-shot acceptance is enforced.");
                }

                var alreadyAccepted = purchaseItem.AcceptanceItems
                    .Where(ai => ai.Acceptance != null && ai.Acceptance.IsPosted)
                    .Sum(ai => ai.QtyAccepted);

                var remaining = approved - alreadyAccepted;
                if (item.QtyAccepted > remaining)
                {
                    throw new InvalidOperationException($"Accepted quantity {item.QtyAccepted} exceeds remaining {remaining} for purchase item {item.PurchaseItemId}.");
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
