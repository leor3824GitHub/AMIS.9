using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;

public sealed class CreateInspectionRequestHandler(
    ILogger<CreateInspectionRequestHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<CreateInspectionRequestCommand, CreateInspectionRequestResponse>
{
    public async Task<CreateInspectionRequestResponse> Handle(CreateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!request.PurchaseId.HasValue || request.PurchaseId.Value == Guid.Empty)
        {
            throw new InvalidOperationException("A purchase must be specified before raising an inspection request.");
        }

        // Enforce: A purchase cannot have multiple open inspection requests unless split across different inspectors.
        // Rule:
        // - If there is an open request with no inspector, block new requests.
        // - If there is an open request with the same inspector, block.
        // - Allow multiple open requests only when each has a different InspectorId.
        var openSpec = new OpenInspectionRequestsByPurchaseSpec(request.PurchaseId.Value);
        var existingOpen = await repository.ListAsync(openSpec, cancellationToken).ConfigureAwait(false);

        if (existingOpen.Count > 0)
        {
            var existsUnassigned = existingOpen.Any(r => r.InspectorId == null);
            if (existsUnassigned)
            {
                throw new InvalidOperationException("An open inspection request for this purchase already exists without an assigned inspector. Use the existing request or assign an inspector to it.");
            }

            if (request.InspectorId == null)
            {
                throw new InvalidOperationException("An open inspection request for this purchase already exists. Provide a different inspector to split responsibilities or use the existing request.");
            }

            var sameInspector = existingOpen.Any(r => r.InspectorId == request.InspectorId);
            if (sameInspector)
            {
                throw new InvalidOperationException("This inspector already has an open inspection request for the selected purchase.");
            }
        }

        var inspectionRequest = InspectionRequest.Create(request.PurchaseId, request.InspectorId);

        await repository.AddAsync(inspectionRequest, cancellationToken);

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
