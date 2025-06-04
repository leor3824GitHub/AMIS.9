using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;

public sealed class CreateInspectionRequestHandler(
    ILogger<CreateInspectionRequestHandler> logger,
    [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    : IRequestHandler<CreateInspectionRequestCommand, CreateInspectionRequestResponse>
{
    public async Task<CreateInspectionRequestResponse> Handle(CreateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspectionRequest = InspectionRequest.Create(
            purchaseId: request.PurchaseId,
            requestedById: request.RequestById
        );

        await repository.AddAsync(inspectionRequest, cancellationToken);
        logger.LogInformation("InspectionRequest created {InspectionRequestId}", inspectionRequest.Id);

        return new CreateInspectionRequestResponse(inspectionRequest.Id);
    }
}
