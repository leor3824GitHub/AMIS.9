using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.UpdateStatus.v1;

public sealed class UpdateInspectionRequestStatusHandler(
    ILogger<UpdateInspectionRequestStatusHandler> logger,
    [FromKeyedServices("inventories:inspectionRequests")] IRepository<InspectionRequest> repository)
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

