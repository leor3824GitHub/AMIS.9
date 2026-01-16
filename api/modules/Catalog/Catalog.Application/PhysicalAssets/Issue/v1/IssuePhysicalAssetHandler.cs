using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.Issue.v1;

/// <summary>
/// Handler for issuing a physical asset to an employee
/// </summary>
public sealed class IssuePhysicalAssetHandler(
    ILogger<IssuePhysicalAssetHandler> logger,
    [FromKeyedServices("catalog:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<IssuePhysicalAssetCommand, IssuePhysicalAssetResponse>
{
    public async Task<IssuePhysicalAssetResponse> Handle(IssuePhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        var history = asset.Issue(request.EmployeeId, request.EmployeeName, request.DocumentNumber, request.QuantityIssued);
        await repository.UpdateAsync(asset, cancellationToken);

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
