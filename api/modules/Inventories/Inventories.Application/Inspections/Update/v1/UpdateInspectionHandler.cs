using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.Inspections.Update.v1;

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
    [FromKeyedServices("inventories:inspections")] IRepository<Inspection> repository,
    [FromKeyedServices("inventories:purchases")] IRepository<Purchase> purchaseRepository)
    : IRequestHandler<UpdateInspectionCommand, UpdateInspectionResponse>
{
    public async Task<UpdateInspectionResponse> Handle(UpdateInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspection ?? throw new InspectionNotFoundException(request.Id);

        // Apply changes using aggregate methods
        if (request.InspectorId.HasValue)
        {
            inspection.SetEmployee(request.InspectorId.Value);
        }

        inspection.SetInspectedOn(request.InspectionDate);
        inspection.UpdateRemarks(request.Remarks);

        // Load purchase with items to re-evaluate inspection status
        if (inspection.PurchaseId.HasValue)
        {
            var purchaseSpec = new PurchaseWithItemsSpec(inspection.PurchaseId.Value);
            var purchase = await purchaseRepository.FirstOrDefaultAsync(purchaseSpec, cancellationToken);
            inspection.EvaluateAndSetStatus(purchase);
        }

        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection {InspectionId} updated (without item changes).", inspection.Id);

        return new UpdateInspectionResponse(inspection.Id);
    }
}

