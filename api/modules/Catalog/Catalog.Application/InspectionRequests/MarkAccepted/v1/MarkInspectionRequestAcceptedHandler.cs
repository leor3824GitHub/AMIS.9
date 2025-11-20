using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkAccepted.v1;

public sealed class MarkInspectionRequestAcceptedHandler(
    ILogger<MarkInspectionRequestAcceptedHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<MarkInspectionRequestAcceptedCommand, MarkInspectionRequestAcceptedResponse>
{
    public async Task<MarkInspectionRequestAcceptedResponse> Handle(MarkInspectionRequestAcceptedCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"InspectionRequest {request.Id} not found");
        inspectionRequest.MarkAccepted();
        await repository.UpdateAsync(inspectionRequest, cancellationToken);
        logger.LogInformation("InspectionRequest {InspectionRequestId} marked accepted.", inspectionRequest.Id);
        return new MarkInspectionRequestAcceptedResponse(inspectionRequest.Id);
    }
}
