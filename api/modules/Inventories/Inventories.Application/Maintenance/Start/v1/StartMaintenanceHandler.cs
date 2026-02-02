using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Start.v1;

/// <summary>
/// Handler for starting maintenance work
/// Verifies maintenance is in Scheduled state
/// Records start timestamp and technician
/// Transitions to InProgress state
/// Note: AssetMaintenance is saved as part of PhysicalAsset aggregate
/// </summary>
public sealed class StartMaintenanceHandler(
    ILogger<StartMaintenanceHandler> logger,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
    : IRequestHandler<StartMaintenanceCommand, StartMaintenanceResponse>
{
    public async Task<StartMaintenanceResponse> Handle(
        StartMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // TODO: Get from current user context
        var currentUserId = Guid.Empty;
        if (currentUserId == Guid.Empty)
            throw new UnauthorizedAccessException("User context is required.");

        // Validate technician exists
        var technician = await employeeRepository.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw new InvalidOperationException($"Employee {currentUserId} not found.");

        // Find maintenance by ID by searching through all assets (normally would use a query/spec)
        // For now, this is a simplified approach - in production you'd use a specification or query
        throw new NotImplementedException("Need to implement maintenance lookup. Use IReadRepository with specification pattern.");
    }
}
