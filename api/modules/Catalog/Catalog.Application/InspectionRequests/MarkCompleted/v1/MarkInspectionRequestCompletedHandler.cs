using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;

public sealed class MarkInspectionRequestCompletedHandler(
    ILogger<MarkInspectionRequestCompletedHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<MarkInspectionRequestCompletedCommand, MarkInspectionRequestCompletedResponse>
{
    public async Task<MarkInspectionRequestCompletedResponse> Handle(MarkInspectionRequestCompletedCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"InspectionRequest {request.Id} not found");
        inspectionRequest.MarkCompleted();
        await repository.UpdateAsync(inspectionRequest, cancellationToken);
        logger.LogInformation("InspectionRequest {InspectionRequestId} marked completed.", inspectionRequest.Id);
        return new MarkInspectionRequestCompletedResponse(inspectionRequest.Id);
    }
}
