using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Create.v1;

public sealed class CreateInspectionRequestHandler(
    ILogger<CreateInspectionRequestHandler> logger,
    [FromKeyedServices("inventories:inspectionRequests")] IRepository<InspectionRequest> inspectionRequestRepository,
    [FromKeyedServices("inventories:purchases")] IRepository<Purchase> purchaseRepository)
    : IRequestHandler<CreateInspectionRequestCommand, CreateInspectionRequestResponse>
{
    public async Task<CreateInspectionRequestResponse> Handle(CreateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!request.PurchaseId.HasValue || request.PurchaseId.Value == Guid.Empty)
        {
            throw new InvalidOperationException("A purchase must be specified before raising an inspection request.");
        }

        var inspectionRequest = InspectionRequest.Create(request.PurchaseId, request.InspectorId);

        await inspectionRequestRepository.AddAsync(inspectionRequest, cancellationToken);

        // Update purchase status to Submitted when inspection request is created
        var purchase = await purchaseRepository.GetByIdAsync(request.PurchaseId.Value, cancellationToken);
        if (purchase != null && purchase.Status == PurchaseStatus.Draft)
        {
            purchase.Submit();
            await purchaseRepository.UpdateAsync(purchase, cancellationToken);
            logger.LogInformation("Purchase {PurchaseId} status updated to Submitted due to inspection request creation", purchase.Id);
        }

        logger.LogInformation(
            "InspectionRequest created {InspectionRequestId}{InspectorInfo}",
            inspectionRequest.Id,
            request.InspectorId is not null
                ? $" with AssignedInspectorId {request.InspectorId}"
                : string.Empty
        );

        return new CreateInspectionRequestResponse(inspectionRequest.Id);
    }
}

