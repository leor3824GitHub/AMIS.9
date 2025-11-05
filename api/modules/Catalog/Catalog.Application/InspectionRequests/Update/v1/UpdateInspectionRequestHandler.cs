using System.Transactions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Update.v1;

public sealed class UpdateInspectionRequestHandler(
    ILogger<UpdateInspectionRequestHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<UpdateInspectionRequestCommand, UpdateInspectionRequestResponse>
{
    public async Task<UpdateInspectionRequestResponse> Handle(UpdateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspectionRequest ?? throw new InspectionRequestNotFoundException(request.Id);
        try
        {
            // If PurchaseId/InspectorId are changing, enforce the same constraint as in creation
            if (request.PurchaseId.HasValue && request.PurchaseId.Value != inspectionRequest.PurchaseId)
            {
                var openSpec = new OpenInspectionRequestsByPurchaseSpec(request.PurchaseId.Value);
                var existingOpen = await repository.ListAsync(openSpec, cancellationToken).ConfigureAwait(false);

                // exclude self if somehow same id with same purchase (safety)
                existingOpen = existingOpen.Where(r => r.Id != inspectionRequest.Id).ToList();

                if (existingOpen.Count > 0)
                {
                    var existsUnassigned = existingOpen.Any(r => r.InspectorId == null);
                    if (existsUnassigned)
                    {
                        throw new InvalidOperationException("Another open inspection request for this purchase exists without an assigned inspector.");
                    }

                    if (request.InspectorId == null)
                    {
                        throw new InvalidOperationException("Another open inspection request for this purchase exists. Provide a different inspector or use the existing request.");
                    }

                    var sameInspector = existingOpen.Any(r => r.InspectorId == request.InspectorId);
                    if (sameInspector)
                    {
                        throw new InvalidOperationException("This inspector already has an open inspection request for the selected purchase.");
                    }
                }
            }

            // If only inspector changes, ensure no duplicate open request for same purchase and inspector
            if (request.InspectorId != inspectionRequest.InspectorId && inspectionRequest.PurchaseId.HasValue)
            {
                var openSpec = new OpenInspectionRequestsByPurchaseSpec(inspectionRequest.PurchaseId.Value);
                var existingOpen = await repository.ListAsync(openSpec, cancellationToken).ConfigureAwait(false);
                existingOpen = existingOpen.Where(r => r.Id != inspectionRequest.Id).ToList();

                if (existingOpen.Any(r => r.InspectorId == null))
                {
                    throw new InvalidOperationException("Cannot change inspector: there is another open request for this purchase without an assigned inspector.");
                }

                if (request.InspectorId != null && existingOpen.Any(r => r.InspectorId == request.InspectorId))
                {
                    throw new InvalidOperationException("This inspector already has an open inspection request for the selected purchase.");
                }
            }

            inspectionRequest.Update(request.PurchaseId, request.InspectorId);

            await repository.UpdateAsync(inspectionRequest, cancellationToken);


            logger.LogInformation(
                "InspectionRequest {InspectionRequestId} successfully updated. Status: {Status}, AssignedInspector: {Inspector}",
                inspectionRequest.Id,
                inspectionRequest.Status,
                inspectionRequest.InspectorId?.ToString() ?? "none"
            );

            return new UpdateInspectionRequestResponse(inspectionRequest.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update InspectionRequest {InspectionRequestId}", request.Id);
            throw;
        }

    }
}
