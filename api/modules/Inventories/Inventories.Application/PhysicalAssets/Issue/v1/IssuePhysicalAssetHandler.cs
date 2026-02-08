using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Issue.v1;

/// <summary>
/// Handler for issuing a physical asset to an employee
/// </summary>
public sealed class IssuePhysicalAssetHandler(
    ILogger<IssuePhysicalAssetHandler> logger,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<IssuePhysicalAssetCommand, IssuePhysicalAssetResponse>
{
    public async Task<IssuePhysicalAssetResponse> Handle(IssuePhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        // Pass signature names for formal PAR workflow (optional for semi-expendable ICS)
        var history = asset.Issue(
            request.EmployeeId,
            request.EmployeeName,
            request.DocumentNumber,
            request.QuantityIssued,
            request.Location,
            emitEvent: true,
            classificationPolicy: null,
            request.IssuedByName,
            request.ReceivedByName,
            request.ApprovedByName);
        
        try
        {
            await repository.UpdateAsync(asset, cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency conflict by reloading the entity
            logger.LogWarning(
                "Concurrency conflict while issuing asset {AssetId}. Reloading and retrying.",
                asset.Id);
            
            // Reload the asset from the database
            var reloadedAsset = await repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");
            
            // Re-apply the issue operation
            history = reloadedAsset.Issue(
                request.EmployeeId,
                request.EmployeeName,
                request.DocumentNumber,
                request.QuantityIssued,
                request.Location,
                emitEvent: true,
                classificationPolicy: null,
                request.IssuedByName,
                request.ReceivedByName,
                request.ApprovedByName);
            
            // Try update again
            await repository.UpdateAsync(reloadedAsset, cancellationToken);
            asset = reloadedAsset;
        }

        logger.LogInformation(
            "Physical asset {AssetId} issued to employee {EmployeeId} ({EmployeeName}) via {DocumentNumber}",
            asset.Id,
            request.EmployeeId,
            request.EmployeeName,
            request.DocumentNumber);

        return new IssuePhysicalAssetResponse
        {
            AssetId = asset.Id,
            AssignmentHistoryId = history.Id,
            EmployeeName = request.EmployeeName,
            DocumentNumber = request.DocumentNumber,
            IssuedDate = history.AssignmentDate,
            Message = "Asset issued successfully"
        };
    }
}

