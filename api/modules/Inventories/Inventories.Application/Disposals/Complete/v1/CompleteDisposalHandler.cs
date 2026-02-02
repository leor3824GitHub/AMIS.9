using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Disposals.Complete.v1;

/// <summary>
/// Handler for completing a disposal
/// Verifies disposal is in Approved state
/// Calculates gain/loss based on book value and salvage value
/// Records financial impact and completes disposal
/// </summary>
public sealed class CompleteDisposalHandler(
    ILogger<CompleteDisposalHandler> logger,
    [FromKeyedServices("inventories:disposals")] IRepository<AssetDisposal> disposalRepository,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
    : IRequestHandler<CompleteDisposalCommand, CompleteDisposalResponse>
{
    public async Task<CompleteDisposalResponse> Handle(
        CompleteDisposalCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // TODO: Get from current user context
        var currentUserId = Guid.Empty;
        if (currentUserId == Guid.Empty)
            throw new UnauthorizedAccessException("User context is required.");

        // Retrieve disposal
        var disposal = await disposalRepository.GetByIdAsync(request.DisposalId, cancellationToken)
            ?? throw new InvalidOperationException($"Disposal {request.DisposalId} not found.");

        // Verify in Approved state
        if (!disposal.CanBeCompleted)
            throw new InvalidOperationException($"Cannot complete disposal with status '{disposal.Status}'. Disposal must be approved first.");

        // Validate completer exists
        var completer = await employeeRepository.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw new InvalidOperationException($"Employee {currentUserId} not found.");

        // Retrieve asset for book value calculation
        var asset = await assetRepository.GetByIdAsync(disposal.PhysicalAssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Asset {disposal.PhysicalAssetId} not found.");

        // Complete disposal
        disposal.Complete(
            completer.Id,
            request.SalvageValue,
            request.DisposalReferenceNumber);

        // Record financial impact (gain/loss)
        disposal.RecordFinancialImpact(asset.BookValue);

        // Save changes
        await disposalRepository.UpdateAsync(disposal, cancellationToken);
        await disposalRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Disposal {DisposalId} (asset {PropertyCode}) completed. BookValue: {BookValue}, SalvageValue: {SalvageValue}, GainOrLoss: {GainOrLoss}",
            disposal.Id,
            disposal.AssetPropertyCode,
            asset.BookValue,
            disposal.SalvageValue,
            disposal.GainOrLoss);

        return new CompleteDisposalResponse(
            disposal.Id,
            disposal.AssetPropertyCode,
            disposal.Status.ToString(),
            disposal.CompletedOn!.Value,
            disposal.SalvageValue,
            disposal.GainOrLoss);
    }
}
