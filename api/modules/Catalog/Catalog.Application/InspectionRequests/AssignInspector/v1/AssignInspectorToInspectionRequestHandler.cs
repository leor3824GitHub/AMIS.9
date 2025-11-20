using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.AssignInspector.v1;

public sealed class AssignInspectorToInspectionRequestHandler(
    ILogger<AssignInspectorToInspectionRequestHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<AssignInspectorToInspectionRequestCommand, AssignInspectorToInspectionRequestResponse>
{
    public async Task<AssignInspectorToInspectionRequestResponse> Handle(AssignInspectorToInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"InspectionRequest {request.Id} not found");
        inspectionRequest.AssignInspector(request.InspectorId);
        await repository.UpdateAsync(inspectionRequest, cancellationToken);
        logger.LogInformation("InspectionRequest {InspectionRequestId} assigned to Inspector {InspectorId}.", inspectionRequest.Id, request.InspectorId);
        return new AssignInspectorToInspectionRequestResponse(inspectionRequest.Id);
    }
}
