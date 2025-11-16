using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Application.Inspections.Specifications; // added for spec

namespace AMIS.WebApi.Catalog.Application.Inspections.Approve.v1;

public sealed class ApproveInspectionHandler(
    ILogger<ApproveInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<ApproveInspectionCommand, ApproveInspectionResponse>
{
    public async Task<ApproveInspectionResponse> Handle(ApproveInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load with items to satisfy domain invariants in Approve()
        var spec = new GetInspectionWithItemsSpec(request.Id);
        var inspection = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        _ = inspection ?? throw new InspectionNotFoundException(request.Id);

        inspection.Approve(); // Domain will validate items & passed status

        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection {InspectionId} approved.", inspection.Id);
        return new ApproveInspectionResponse(inspection.Id);
    }
}
