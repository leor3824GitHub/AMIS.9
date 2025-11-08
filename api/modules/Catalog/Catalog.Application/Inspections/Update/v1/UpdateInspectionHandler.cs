using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Update.v1;

// Specification to load Purchase with Items
internal sealed class PurchaseWithItemsSpec : Specification<Purchase>
{
    public PurchaseWithItemsSpec(Guid purchaseId)
    {
        Query
         .Where(p => p.Id == purchaseId)
       .Include(p => p.Items);
    }
}

public sealed class UpdateInspectionHandler(
    ILogger<UpdateInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository,
    [FromKeyedServices("catalog:inspectionRequests")] IReadRepository<InspectionRequest> inspectionRequestRepo,
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> purchaseRepo)
    : IRequestHandler<UpdateInspectionCommand, UpdateInspectionResponse>
{
    public async Task<UpdateInspectionResponse> Handle(UpdateInspectionCommand request, CancellationToken cancellationToken)
    {
    ArgumentNullException.ThrowIfNull(request);

        // Load aggregate root WITHOUT eager loading of navigation properties
        var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
      _ = inspection ?? throw new InspectionNotFoundException(request.Id);

        // Apply changes using aggregate methods
 if (request.InspectorId.HasValue)
      {
      inspection.SetEmployee(request.InspectorId.Value);
   }

        inspection.SetInspectedOn(request.InspectionDate);
  inspection.UpdateRemarks(request.Remarks);

        // Re-evaluate inspection status - fetch related data from their own repositories
        // This follows DDD principle: aggregates reference each other by ID only
        // Access InspectionRequestId (public property) instead of InspectionRequest (private navigation)
        InspectionRequest? inspectionRequest = null;
        if (inspection.InspectionRequestId.HasValue)
        {
            inspectionRequest = await inspectionRequestRepo.GetByIdAsync(inspection.InspectionRequestId.Value, cancellationToken);
        }
        
        if (inspectionRequest?.PurchaseId != null)
        {
     var purchaseSpec = new PurchaseWithItemsSpec(inspectionRequest.PurchaseId.Value);
    var purchase = await purchaseRepo.FirstOrDefaultAsync(purchaseSpec, cancellationToken);
     
  if (purchase != null)
            {
     // Pass data (not entity) across aggregate boundary
                var purchaseItemData = purchase.Items
       .Select(pi => new PurchaseItemSummary(pi.Id, pi.Qty))
         .ToList();
     
 inspection.EvaluateAndSetStatus(purchaseItemData);
   }
        }

   await repository.UpdateAsync(inspection, cancellationToken);
    logger.LogInformation("Inspection {InspectionId} updated.", inspection.Id);

        return new UpdateInspectionResponse(inspection.Id);
    }
}
