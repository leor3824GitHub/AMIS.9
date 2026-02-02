using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Disposals.Approve.v1;

/// <summary>
/// Handler for approving a disposal request
/// Verifies disposal is in Pending state
/// Records approver and approval timestamp
/// Transitions to Approved state
/// </summary>
public sealed class ApproveDisposalHandler(
    ILogger<ApproveDisposalHandler> logger,
    [FromKeyedServices("inventories:disposals")] IRepository<AssetDisposal> disposalRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
    : IRequestHandler<ApproveDisposalCommand, ApproveDisposalResponse>
{
    public async Task<ApproveDisposalResponse> Handle(
        ApproveDisposalCommand request,
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

        // Verify in Pending state
        if (!disposal.IsApprovalPending)
            throw new InvalidOperationException($"Cannot approve disposal with status '{disposal.Status}'. Disposal must be pending approval.");

        // Validate approver exists
        var approver = await employeeRepository.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw new InvalidOperationException($"Employee {currentUserId} not found.");

        // Approve disposal
        disposal.Approve(approver.Id, request.ApprovalNotes);

        // Save changes
        await disposalRepository.UpdateAsync(disposal, cancellationToken);
        await disposalRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Disposal {DisposalId} (asset {PropertyCode}) approved by employee {EmployeeId}",
            disposal.Id,
            disposal.AssetPropertyCode,
            approver.Id);

        return new ApproveDisposalResponse(
            disposal.Id,
            disposal.AssetPropertyCode,
            disposal.Status.ToString(),
            disposal.ApprovedOn!.Value);
    }
}
