using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Exceptions;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Specifications;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

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

        var spec = new PhysicalAssetByIdSpec(request.Id);
        var asset = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new FshException(
                $"Physical asset {request.Id} not found",
                new[] { $"Asset with ID {request.Id} does not exist" },
                HttpStatusCode.NotFound);

        // Pass signature names for formal PAR workflow (optional for semi-expendable ICS)
        AssetAssignmentHistory history;
        try
        {
            history = asset.Issue(
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
        }
        catch (InvalidOperationException ex)
        {
            // Convert domain validation errors to proper HTTP responses
            throw new FshException(
                ex.Message,
                new[] { ex.Message },
                ex.Message.Contains("already assigned", StringComparison.OrdinalIgnoreCase) 
                    ? HttpStatusCode.Conflict  // 409 for already assigned
                    : HttpStatusCode.BadRequest); // 400 for other validation errors
        }
        catch (ArgumentException ex)
        {
            throw new FshException(
                ex.Message,
                new[] { ex.Message },
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            await repository.UpdateAsync(asset, cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            logger.LogWarning(
                "Concurrency conflict while issuing asset {AssetId}.",
                asset.Id);

            throw new FshException(
                "Asset was updated by another process. Refresh and try again.",
                new[] { "Asset was updated by another process. Refresh and try again." },
                HttpStatusCode.Conflict);
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

