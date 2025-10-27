using AMIS.Framework.Core.Identity.Users.Abstractions;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Services.v1;

public interface IInspectionAuditService
{
    Task LogInspectionRequestCreated(Guid requestId, string requester);
    Task LogInspectionCreated(Guid inspectionId, Guid requestId, string inspector);
    Task LogStatusChange(Guid requestId, string fromStatus, string toStatus, string changedBy);
    Task LogInspectionCompleted(Guid inspectionId, string result, string inspector);
}

public class InspectionAuditService : IInspectionAuditService
{
    private readonly ILogger<InspectionAuditService> _logger;
    private readonly ICurrentUser _currentUser;

    public InspectionAuditService(ILogger<InspectionAuditService> logger, ICurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task LogInspectionRequestCreated(Guid requestId, string requester)
    {
        _logger.LogInformation(
            "Inspection request {RequestId} created by {Requester} at {Timestamp}",
            requestId, requester, DateTime.UtcNow);

        await Task.CompletedTask;
    }

    public async Task LogInspectionCreated(Guid inspectionId, Guid requestId, string inspector)
    {
        _logger.LogInformation(
            "Inspection {InspectionId} created for request {RequestId} by {Inspector} at {Timestamp}",
            inspectionId, requestId, inspector, DateTime.UtcNow);

        await Task.CompletedTask;
    }

    public async Task LogStatusChange(Guid requestId, string fromStatus, string toStatus, string changedBy)
    {
        _logger.LogInformation(
            "Inspection request {RequestId} status changed from {FromStatus} to {ToStatus} by {ChangedBy} at {Timestamp}",
            requestId, fromStatus, toStatus, changedBy, DateTime.UtcNow);

        await Task.CompletedTask;
    }

    public async Task LogInspectionCompleted(Guid inspectionId, string result, string inspector)
    {
        _logger.LogInformation(
            "Inspection {InspectionId} completed with result {Result} by {Inspector} at {Timestamp}",
            inspectionId, result, inspector, DateTime.UtcNow);

        await Task.CompletedTask;
    }
}
