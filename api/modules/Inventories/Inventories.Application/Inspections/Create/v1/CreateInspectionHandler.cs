using System;
using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.Inspections.Create.v1;

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
    [FromKeyedServices("inventories:inspections")] IRepository<Inspection> repository,
    [FromKeyedServices("inventories:purchases")] IRepository<Purchase> purchaseRepository,
    [FromKeyedServices("inventories:physicalAssets")] IRepository<PhysicalAsset> assetRepository)
    : IRequestHandler<CreateInspectionCommand, CreateInspectionResponse>
{
    public async Task<CreateInspectionResponse> Handle(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Inspection inspection = request.Type switch
        {
            InspectionType.NewDelivery => await CreateNewDeliveryInspection(request, cancellationToken),
            InspectionType.AssetReturn => await CreateAssetReturnInspection(request, cancellationToken),
            InspectionType.Repair => await CreateRepairInspection(request, cancellationToken),
            _ => throw new InvalidOperationException($"Unknown inspection type: {request.Type}")
        };

        await repository.AddAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection created: {InspectionId} (Type: {Type})", inspection.Id, request.Type);

        return new CreateInspectionResponse(inspection.Id);
    }

    private async Task<Inspection> CreateNewDeliveryInspection(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        if (!request.PurchaseId.HasValue || request.PurchaseId.Value == Guid.Empty)
            throw new InvalidOperationException("PurchaseId is required for NewDelivery inspections.");

        var inspection = Inspection.CreateForNewDelivery(
            purchaseId: request.PurchaseId.Value,
            employeeId: request.EmployeeId,
            inspectedOn: request.InspectedOn,
            remarks: request.Remarks,
            iarDocumentPath: request.IARDocumentPath
        );

        // Add items if provided
        if (request.Items is not null)
        {
            foreach (var item in request.Items)
            {
                var status = item.InspectionItemStatus ?? InspectionItemStatus.NotInspected;
                _ = inspection.AddItem(item.PurchaseItemId, item.QtyInspected, item.QtyPassed, item.QtyFailed, item.Remarks, status);
            }

            // Evaluate status based on purchase items
            var purchaseSpec = new PurchaseWithItemsSpec(request.PurchaseId.Value);
            var purchase = await purchaseRepository.FirstOrDefaultAsync(purchaseSpec, cancellationToken);
            inspection.EvaluateAndSetStatus(purchase);
        }

        return inspection;
    }

    private async Task<Inspection> CreateAssetReturnInspection(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        if (!request.PhysicalAssetId.HasValue || request.PhysicalAssetId.Value == Guid.Empty)
            throw new InvalidOperationException("PhysicalAssetId is required for AssetReturn inspections.");

        var asset = await assetRepository.GetByIdAsync(request.PhysicalAssetId.Value, cancellationToken);
        if (asset is null)
            throw new InvalidOperationException($"Physical asset {request.PhysicalAssetId} not found.");

        return Inspection.CreateForAssetReturn(
            physicalAssetId: request.PhysicalAssetId.Value,
            employeeId: request.EmployeeId,
            inspectedOn: request.InspectedOn,
            remarks: request.Remarks
        );
    }

    private async Task<Inspection> CreateRepairInspection(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        if (!request.PhysicalAssetId.HasValue || request.PhysicalAssetId.Value == Guid.Empty)
            throw new InvalidOperationException("PhysicalAssetId is required for Repair inspections.");

        var asset = await assetRepository.GetByIdAsync(request.PhysicalAssetId.Value, cancellationToken);
        if (asset is null)
            throw new InvalidOperationException($"Physical asset {request.PhysicalAssetId} not found.");

        return Inspection.CreateForRepair(
            physicalAssetId: request.PhysicalAssetId.Value,
            employeeId: request.EmployeeId,
            inspectedOn: request.InspectedOn,
            remarks: request.Remarks
        );
    }
}


