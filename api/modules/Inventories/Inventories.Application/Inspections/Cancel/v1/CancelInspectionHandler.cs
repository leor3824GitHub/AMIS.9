using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Inspections.Cancel.v1;

public sealed class CancelInspectionHandler(
    ILogger<CancelInspectionHandler> logger,
    [FromKeyedServices("inventories:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<CancelInspectionCommand, CancelInspectionResponse>
{
    public async Task<CancelInspectionResponse> Handle(CancelInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspection ?? throw new InspectionNotFoundException(request.Id);
        inspection.Cancel(request.Reason);
        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection {InspectionId} cancelled.", inspection.Id);
        return new CancelInspectionResponse(inspection.Id);
    }
}

