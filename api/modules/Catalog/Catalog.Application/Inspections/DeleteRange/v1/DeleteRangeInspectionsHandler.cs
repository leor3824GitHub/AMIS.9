using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AMIS.WebApi.Catalog.Application.Inspections.DeleteRange.v1
{
    public sealed class DeleteRangeInspectionsHandler(
        ILogger<DeletePurchaseHandler> logger,
        [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
        : IRequestHandler<DeleteRangeInspectionsCommand>
    {       
        public async Task Handle(DeleteRangeInspectionsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var inspections = new List<Inspection>();

            foreach (var inspectionIds in request.InspectionIds)
            {
                var inspection = await repository.GetByIdAsync(inspectionIds, cancellationToken);
                if (inspection != null)
                {
                    inspections.Add(inspection);
                }
            }

            if (inspections.Count == 0)
            {
                logger.LogInformation("No inspections found for the provided {InspectionCount} IDs", inspections.Count);
                //throw new PurchaseNotFoundException("No purchases found for the provided IDs.");
            }

            await repository.DeleteRangeAsync(inspections, cancellationToken);
            logger.LogInformation("{InspectionCount} inspections deleted", inspections.Count);
        }
    }
}
