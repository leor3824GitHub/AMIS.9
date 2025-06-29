using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AMIS.WebApi.Catalog.Application.InspectionRequests.DeleteRange.v1
{
    public sealed class DeleteRangeInspectionRequestsHandler(
        ILogger<DeletePurchaseHandler> logger,
        [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
        : IRequestHandler<DeleteRangeInspectionRequestsCommand>
    {       
        public async Task Handle(DeleteRangeInspectionRequestsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var inspectionRequests = new List<InspectionRequest>();

            foreach (var inspectionIds in request.InspectionRequestIds)
            {
                var inspectionRequest = await repository.GetByIdAsync(inspectionIds, cancellationToken);
                if (inspectionRequest != null &&
                    inspectionRequest.Status == InspectionRequestStatus.Pending &&
                    (!inspectionRequest.InspectorId.HasValue || inspectionRequest.InspectorId.Value == Guid.Empty))
                {
                    inspectionRequests.Add(inspectionRequest);
                }
            }

            if (inspectionRequests.Count == 0)
            {
                logger.LogInformation("No inspection requests found for the provided {InspectionRequestCount} IDs", inspectionRequests.Count);
                //throw new PurchaseNotFoundException("No purchases found for the provided IDs.");
            }

            await repository.DeleteRangeAsync(inspectionRequests, cancellationToken);
            logger.LogInformation("{InspectionCount} inspection requests deleted", inspectionRequests.Count);
        }
    }
}
