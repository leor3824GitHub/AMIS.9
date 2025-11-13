using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Inspections.Reject.v1;

public sealed class RejectInspectionHandler(
    ILogger<RejectInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<RejectInspectionCommand, RejectInspectionResponse>
{
    public async Task<RejectInspectionResponse> Handle(RejectInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspection ?? throw new InspectionNotFoundException(request.Id);
        inspection.Reject(request.Reason);
        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection {InspectionId} rejected.", inspection.Id);
        return new RejectInspectionResponse(inspection.Id);
    }
}
