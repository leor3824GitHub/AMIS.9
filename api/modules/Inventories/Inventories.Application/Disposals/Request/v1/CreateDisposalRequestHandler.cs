using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Disposals.Request.v1;

/// <summary>
/// Handler for creating a disposal request
/// Validates asset existence, disposal method, and condition
/// Creates AssetDisposal aggregate in Pending state
/// </summary>
public sealed class CreateDisposalRequestHandler(
    ILogger<CreateDisposalRequestHandler> logger,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:disposals")] IRepository<AssetDisposal> disposalRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
    : IRequestHandler<CreateDisposalRequestCommand, CreateDisposalRequestResponse>
{
    public async Task<CreateDisposalRequestResponse> Handle(
        CreateDisposalRequestCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // TODO: Get from current user context
        var currentUserId = Guid.Empty;
        if (currentUserId == Guid.Empty)
            throw new UnauthorizedAccessException("User context is required.");

        // Validate asset exists
        var asset = await assetRepository.GetByIdAsync(request.PhysicalAssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.PhysicalAssetId} not found.");

        // Validate disposal method
        if (!DisposalMethod.TryParse(request.DisposalMethod, out var disposalMethod) || disposalMethod == null)
            throw new ArgumentException($"Invalid disposal method: {request.DisposalMethod}");

        // Validate asset condition
        if (!AssetCondition.TryParse(request.AssetConditionAtDisposal, out var assetCondition) || assetCondition == null)
            throw new ArgumentException($"Invalid asset condition: {request.AssetConditionAtDisposal}");

        // Validate requesting employee has permission
        var employee = await employeeRepository.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw new InvalidOperationException($"Employee {currentUserId} not found.");

        // Create disposal request
        var disposal = AssetDisposal.CreateRequest(
            asset.Id,
            asset.PropertyCode,
            asset.Description,
            employee.Id,
            disposalMethod,
            assetCondition,
            request.JustificationReason);

        // Save to repository
        await disposalRepository.AddAsync(disposal, cancellationToken);
        await disposalRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Disposal request created for asset {AssetId} ({PropertyCode}) by employee {EmployeeId}",
            asset.Id,
            asset.PropertyCode,
            employee.Id);

        return new CreateDisposalRequestResponse(
            disposal.Id,
            disposal.AssetPropertyCode,
            disposal.Status.ToString());
    }
}
