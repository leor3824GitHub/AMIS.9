using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Quarantine.v1;

internal sealed class QuarantineInspectionHandler(
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<QuarantineInspectionCommand, QuarantineInspectionResponse>
{
    public async Task<QuarantineInspectionResponse> Handle(QuarantineInspectionCommand request, CancellationToken cancellationToken)
    {
        var inspection = await repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Inspection with ID {request.InspectionId} not found.");

        inspection.Quarantine(request.Reason);

        await repository.SaveChangesAsync(cancellationToken);

        return new QuarantineInspectionResponse(
            inspection.Id,
            inspection.Status.ToString(),
            $"Inspection quarantined: {request.Reason}");
    }
}
