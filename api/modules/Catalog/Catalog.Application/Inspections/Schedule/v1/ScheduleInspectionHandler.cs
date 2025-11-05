using MediatR;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Schedule.v1;

internal sealed class ScheduleInspectionHandler(
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<ScheduleInspectionCommand, ScheduleInspectionResponse>
{
    public async Task<ScheduleInspectionResponse> Handle(ScheduleInspectionCommand request, CancellationToken cancellationToken)
    {
        var inspection = await repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Inspection with ID {request.InspectionId} not found.");

        inspection.Schedule(request.ScheduledDate);

        await repository.SaveChangesAsync(cancellationToken);

        return new ScheduleInspectionResponse(
            inspection.Id,
            inspection.Status.ToString(),
            inspection.InspectedOn,
            "Inspection scheduled successfully.");
    }
}
