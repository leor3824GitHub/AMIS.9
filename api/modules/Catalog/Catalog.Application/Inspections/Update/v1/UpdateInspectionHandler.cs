using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Update.v1;

public sealed class UpdateInspectionHandler(
    ILogger<UpdateInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<UpdateInspectionCommand, UpdateInspectionResponse>
{
    public async Task<UpdateInspectionResponse> Handle(UpdateInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspection ?? throw new InspectionNotFoundException(request.Id);

        inspection.Update(request.InspectorId, request.InspectionRequestId, request.InspectionDate, request.Remarks);

        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection {InspectionId} updated (without item changes).", inspection.Id);

        return new UpdateInspectionResponse(inspection.Id);
    }
}
