﻿using System.Transactions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Update.v1;

public sealed class UpdateInspectionRequestHandler(
    ILogger<UpdateInspectionRequestHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<UpdateInspectionRequestCommand, UpdateInspectionRequestResponse>
{
    public async Task<UpdateInspectionRequestResponse> Handle(UpdateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspectionRequest ?? throw new InspectionRequestNotFoundException(request.Id);
        try
        {
            inspectionRequest.Update(request.PurchaseId, request.InspectorId, request.Status);

            await repository.UpdateAsync(inspectionRequest, cancellationToken);


            logger.LogInformation(
                "InspectionRequest {InspectionRequestId} successfully updated. Status: {Status}, AssignedInspector: {Inspector}",
                inspectionRequest.Id,
                inspectionRequest.Status,
                inspectionRequest.InspectorId?.ToString() ?? "none"
            );

            return new UpdateInspectionRequestResponse(inspectionRequest.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update InspectionRequest {InspectionRequestId}", request.Id);
            throw;
        }

    }
}
