using System;
using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Create.v1;

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

public sealed class CreateInspectionHandler(
    ILogger<CreateInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository,
  [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> inspectionRequestRepo,
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> purchaseRepo)
    : IRequestHandler<CreateInspectionCommand, CreateInspectionResponse>
{
    public async Task<CreateInspectionResponse> Handle(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load inspection request by ID (not through navigation)
        var inspectionRequest = await inspectionRequestRepo.GetByIdAsync(request.InspectionRequestId, cancellationToken);
        
        if (inspectionRequest is null)
   {
            throw new InvalidOperationException("Create an inspection request before recording an inspection.");
        }

   if (inspectionRequest.PurchaseId != request.PurchaseId)
        {
throw new InvalidOperationException("The inspection must target the same purchase as its inspection request.");
        }

        if (inspectionRequest.Status is InspectionRequestStatus.Pending)
        {
       throw new InvalidOperationException("Assign an inspector to the request before creating an inspection.");
        }

    if (inspectionRequest.Status is InspectionRequestStatus.Completed or InspectionRequestStatus.Accepted)
        {
      throw new InvalidOperationException("This inspection request has already been completed. Create a new request for additional inspections.");
        }

      // Create inspection with InspectionRequestId only (following aggregate boundaries)
        var inspection = Inspection.Create(
   inspectionRequestId: request.InspectionRequestId,
  employeeId: request.InspectorId,
       inspectedOn: request.InspectionDate,
         approved: false,
          remarks: request.Remarks
        );

        if (request.Items is not null)
        {
   foreach (var item in request.Items)
        {
   var status = item.InspectionItemStatus ?? InspectionItemStatus.NotInspected;
     _ = inspection.AddItem(item.PurchaseItemId, item.QtyInspected, item.QtyPassed, item.QtyFailed, item.Remarks, status);
       }
        }

        await repository.AddAsync(inspection, cancellationToken);

  // Evaluate inspection status - fetch purchase data explicitly from its own repository
        if (inspectionRequest.PurchaseId.HasValue)
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

        // Update inspection request status (use IRepository not IReadRepository)
if (inspectionRequest.Status == InspectionRequestStatus.Assigned)
        {
            inspectionRequest.UpdateStatus(InspectionRequestStatus.InProgress);
     await inspectionRequestRepo.UpdateAsync(inspectionRequest, cancellationToken);
        }

        logger.LogInformation("Inspection created {InspectionId} for InspectionRequest {InspectionRequestId}", 
            inspection.Id, request.InspectionRequestId);

     return new CreateInspectionResponse(inspection.Id);
    }
}
