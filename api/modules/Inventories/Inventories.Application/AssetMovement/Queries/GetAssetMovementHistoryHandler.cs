using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.AssetMovement;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.AssetMovement.Queries;

public sealed class GetAssetMovementHistoryHandler(
    [FromKeyedServices("inventories:physicalassets")] IReadRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:par")] IReadRepository<global::AMIS.WebApi.Inventories.Domain.PropertyAcknowledgementReceipt> parRepository,
    [FromKeyedServices("inventories:ics")] IReadRepository<InventoryCustodianSlip> icsRepository,
    [FromKeyedServices("inventories:ppeir")] IReadRepository<PPEIR> ppeIrRepository,
    [FromKeyedServices("inventories:smir")] IReadRepository<SuppliesAndMaterialsIssuanceReport> smirRepository)
    : IRequestHandler<GetAssetMovementHistoryQuery, AssetMovementSummaryDto?>
{
    public async Task<AssetMovementSummaryDto?> Handle(
        GetAssetMovementHistoryQuery request,
        CancellationToken cancellationToken)
    {
        // Find the asset
        var asset = await assetRepository.GetByIdAsync(request.AssetId, cancellationToken);
        if (asset == null)
            return null;

        var movements = new List<AssetMovementEntryDto>();

        // 1. Find all PAR entries for this asset
        var pars = await parRepository.ListAsync(cancellationToken);
        var parMovements = pars
            .Where(p => p.LineItems.Any(li => li.PropertyCode == asset.PropertyCode))
            .SelectMany(p => p.LineItems
                .Where(li => li.PropertyCode == asset.PropertyCode)
                .Select(li => new AssetMovementEntryDto
                {
                    Id = p.Id,
                    DocumentNumber = p.PARNumber,
                    DocumentType = "PAR",
                    Date = p.IssuanceDate,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.Employee?.Name ?? "Unknown",
                    MovementType = p.Status == PARStatus.Posted ? "Issuance" : (p.Status == PARStatus.Returned ? "Return" : p.Status.ToString()),
                    Quantity = 1,  // PPE is always 1
                    Location = p.IssuanceLocation,
                    Condition = p.Status == PARStatus.Returned ? "Returned" : null,
                    Remarks = p.ReturnRemarks,
                    IsActive = p.Status == PARStatus.Posted
                }))
            .ToList();
        movements.AddRange(parMovements);

        // 2. Find all ICS entries for this asset
        var icsList = await icsRepository.ListAsync(cancellationToken);
        var icsMovements = icsList
            .Where(i => i.LineItems.Any(li => li.PropertyCode == asset.PropertyCode))
            .SelectMany(i => i.LineItems
                .Where(li => li.PropertyCode == asset.PropertyCode)
                .Select(li => new AssetMovementEntryDto
                {
                    Id = i.Id,
                    DocumentNumber = i.ICSNumber,
                    DocumentType = "ICS",
                    Date = i.IssuanceDate,
                    EmployeeId = i.EmployeeId,
                    EmployeeName = i.Employee?.Name ?? "Unknown",
                    MovementType = i.Status == ICSStatus.Posted ? "Issuance" : (i.Status == ICSStatus.Returned ? "Return" : i.Status.ToString()),
                    Quantity = li.Quantity,
                    Location = i.IssuanceLocation,
                    Condition = i.Status == ICSStatus.Returned ? "Returned" : null,
                    Remarks = i.ReturnRemarks,
                    IsActive = i.Status == ICSStatus.Posted
                }))
            .ToList();
        movements.AddRange(icsMovements);

        // 3. Find all PPEIR entries (transfers)
        var ppeIrs = await ppeIrRepository.ListAsync(cancellationToken);
        var ppeIrMovements = ppeIrs
            .Where(p => p.Items.Any(item => item.PropertyCode == asset.PropertyCode) && p.Status == PpeReportStatus.Posted)
            .SelectMany(p => p.Items
                .Where(item => item.PropertyCode == asset.PropertyCode)
                .Select(item => new AssetMovementEntryDto
                {
                    Id = p.Id,
                    DocumentNumber = p.IRNumber,
                    DocumentType = "PPEIR",
                    Date = p.Date,
                    EmployeeId = Guid.Empty,  // PPEIR may not have employee, it's location-based
                    EmployeeName = p.IssuedTo,
                    MovementType = "Transfer",
                    Quantity = 1,
                    Location = p.Address,
                    Remarks = p.Notes,
                    IsActive = false  // Transfers don't mark as final custody
                }))
            .ToList();
        movements.AddRange(ppeIrMovements);

        // 4. Find all SMIR entries (transfers for consumables/semex)
        var smirs = await smirRepository.ListAsync(cancellationToken);
        var smirMovements = smirs
            .Where(s => s.LineItems.Any(li => li.PropertyCode == asset.PropertyCode))
            .SelectMany(s => s.LineItems
                .Where(li => li.PropertyCode == asset.PropertyCode)
                .Select(li => new AssetMovementEntryDto
                {
                    Id = s.Id,
                    DocumentNumber = s.SmirNumber,
                    DocumentType = "SMIR",
                    Date = s.TransactionDate,
                    EmployeeId = Guid.Empty,
                    EmployeeName = s.Recipient?.Name ?? "Unknown",
                    MovementType = "Transfer",
                    Quantity = (int)li.Quantity,
                    Location = null,
                    Remarks = s.Notes,
                    IsActive = false
                }))
            .ToList();
        movements.AddRange(smirMovements);

        // Sort by date
        movements = movements.OrderBy(m => m.Date).ToList();

        // Determine current holder (most recent active PAR/ICS)
        var currentMovement = movements
            .Where(m => (m.DocumentType == "PAR" || m.DocumentType == "ICS") && m.IsActive)
            .OrderByDescending(m => m.Date)
            .FirstOrDefault();

        var currentStatus = currentMovement?.IsActive == true ? "Active" : "Returned";

        return new AssetMovementSummaryDto
        {
            AssetId = asset.Id,
            PropertyCode = asset.PropertyCode,
            AssetDescription = $"{asset.PropertyCode} - {asset.ModelNumber}",
            AcquisitionCost = asset.AcquisitionCost,
            CurrentHolderId = currentMovement?.EmployeeId,
            CurrentHolderName = currentMovement?.EmployeeName,
            CurrentStatus = currentStatus,
            CurrentAsOfDate = currentMovement?.Date,
            MovementHistory = movements
        };
    }
}
