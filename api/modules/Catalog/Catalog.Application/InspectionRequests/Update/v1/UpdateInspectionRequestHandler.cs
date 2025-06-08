using System.Transactions;
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
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            inspectionRequest.Update(request.PurchaseId, request.RequestedById, request.AssignedInspectorId, request.Status);

            await repository.UpdateAsync(inspectionRequest, cancellationToken);
            logger.LogInformation("InspectionRequest {InspectionRequestId} updated (without item changes).", inspectionRequest.Id);

            scope.Complete();
            return new UpdateInspectionRequestResponse(inspectionRequest.Id);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Validation failed for InspectionRequest {InspectionRequestId}.", request.Id);
            throw;
        }
        
    }
}
