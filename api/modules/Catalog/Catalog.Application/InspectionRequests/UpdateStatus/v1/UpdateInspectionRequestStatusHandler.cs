using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.UpdateStatus.v1;

public sealed class UpdateInspectionRequestStatusHandler(
    ILogger<UpdateInspectionRequestStatusHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<UpdateInspectionRequestStatusCommand, UpdateInspectionRequestStatusResponse>
{
    public async Task<UpdateInspectionRequestStatusResponse> Handle(UpdateInspectionRequestStatusCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"InspectionRequest {request.Id} not found");
        inspectionRequest.UpdateStatus(request.Status);
        await repository.UpdateAsync(inspectionRequest, cancellationToken);
        logger.LogInformation("InspectionRequest {InspectionRequestId} status updated to {Status}.", inspectionRequest.Id, request.Status);
        return new UpdateInspectionRequestStatusResponse(inspectionRequest.Id);
    }
}
