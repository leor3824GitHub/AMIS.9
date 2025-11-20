using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Acceptances.LinkInspection.v1;

public sealed class LinkAcceptanceInspectionHandler(
    ILogger<LinkAcceptanceInspectionHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> acceptanceRepository,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> inspectionRepository)
    : IRequestHandler<LinkAcceptanceInspectionCommand, LinkAcceptanceInspectionResponse>
{
    public async Task<LinkAcceptanceInspectionResponse> Handle(LinkAcceptanceInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var acceptance = await acceptanceRepository.GetByIdAsync(request.AcceptanceId, cancellationToken) ?? throw new Exception($"Acceptance {request.AcceptanceId} not found");
        var inspection = await inspectionRepository.GetByIdAsync(request.InspectionId, cancellationToken) ?? throw new Exception($"Inspection {request.InspectionId} not found");

        if (inspection.Status != Domain.ValueObjects.InspectionStatus.Approved)
        {
            throw new InvalidOperationException("Only approved inspections can be linked to an acceptance.");
        }

        acceptance.LinkInspection(request.InspectionId);
        acceptance.ValidateAgainstInspection(inspection);

        await acceptanceRepository.UpdateAsync(acceptance, cancellationToken);
        logger.LogInformation("Acceptance {AcceptanceId} linked to Inspection {InspectionId}.", acceptance.Id, inspection.Id);
        return new LinkAcceptanceInspectionResponse(acceptance.Id);
    }
}
