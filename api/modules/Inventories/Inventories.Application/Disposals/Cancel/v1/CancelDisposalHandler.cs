using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Disposals.Cancel.v1;

/// <summary>
/// Handler for cancelling a disposal
/// Can cancel from Pending or Approved states
/// Records cancellation reason for audit trail
/// </summary>
public sealed class CancelDisposalHandler(
    ILogger<CancelDisposalHandler> logger,
    [FromKeyedServices("inventories:disposals")] IRepository<AssetDisposal> disposalRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
    : IRequestHandler<CancelDisposalCommand, CancelDisposalResponse>
{
    public async Task<CancelDisposalResponse> Handle(
        CancelDisposalCommand request,
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

        // Verify can be cancelled
        if (!disposal.CanBeCancelled)
            throw new InvalidOperationException($"Cannot cancel disposal with status '{disposal.Status}'. Only pending or approved disposals can be cancelled.");

        // Validate canceller exists
        var canceller = await employeeRepository.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw new InvalidOperationException($"Employee {currentUserId} not found.");

        // Cancel disposal
        disposal.Cancel(canceller.Id, request.CancellationReason);

        // Save changes
        await disposalRepository.UpdateAsync(disposal, cancellationToken);
        await disposalRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Disposal {DisposalId} (asset {PropertyCode}) cancelled by employee {EmployeeId}. Reason: {Reason}",
            disposal.Id,
            disposal.AssetPropertyCode,
            canceller.Id,
            request.CancellationReason ?? "Not provided");

        return new CancelDisposalResponse(
            disposal.Id,
            disposal.AssetPropertyCode,
            disposal.Status.ToString(),
            disposal.CancelledOn!.Value);
    }
}
