using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications;

namespace AMIS.WebApi.Catalog.Application.Inspections.Complete.v1;

public sealed class CompleteInspectionHandler(
    ILogger<CompleteInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<CompleteInspectionCommand, CompleteInspectionResponse>
{
    public async Task<CompleteInspectionResponse> Handle(CompleteInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load with items (and purchase if needed) to satisfy completion rules
        var spec = new GetInspectionWithItemsSpec(request.Id);
        var inspection = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        _ = inspection ?? throw new InspectionNotFoundException(request.Id);

        inspection.Complete();

        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection {InspectionId} completed.", inspection.Id);
        return new CompleteInspectionResponse(inspection.Id);
    }
}
