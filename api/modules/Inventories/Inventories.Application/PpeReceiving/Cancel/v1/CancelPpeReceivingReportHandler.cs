using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Cancel.v1;

public sealed class CancelPpeReceivingReportHandler(
    [FromKeyedServices("inventories:pperr")] IRepository<Domain.PPERR> repository,
    [FromKeyedServices("inventories:inventory-registries")] IRepository<Domain.InventoryRegistry> registryRepository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<Domain.InventoryTransactionLog> transactionLogRepository,
    IAuthorizationService authorizationService,
    ILogger<CancelPpeReceivingReportHandler> logger) : IRequestHandler<CancelPpeReceivingReportCommand, CancelPpeReceivingReportResponse>
{
    public async Task<CancelPpeReceivingReportResponse> Handle(CancelPpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        // Authorization check: Only users with Cancel permission can cancel reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.Pper}.{FshActions.Cancel}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized cancellation attempt for PPERR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to cancel PPE Receiving reports. Only accounting personnel can perform this action.");
        }

        var report = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (report is null)
        {
            logger.LogWarning("PPE Receiving Report {Id} not found for cancellation", request.Id);
            throw new InvalidOperationException($"PPE Receiving Report {request.Id} not found.");
        }

        // Only allow cancellation of Posted reports
        if (report.Status != Domain.ValueObjects.PpeReportStatus.Posted)
        {
            logger.LogWarning("Cannot cancel PPERR {Id} with status {Status}", request.Id, report.Status);
            throw new InvalidOperationException($"Only Posted reports can be cancelled. Current status: {report.Status}");
        }

        // Cancel the report
        report.Cancel();

        // Create reversal entries in transaction log
        // For receiving (which added items), we create failure entries to document the reversal
        var transactionLogs = new List<Domain.InventoryTransactionLog>();
        foreach (var lineItem in report.Items)
        {
            var reversalLog = Domain.InventoryTransactionLog.CreateFailure(
                propertyCode: lineItem.PropertyCode,
                transactionType: "PPERR",
                reportNumber: report.RRNumber,
                inventoryBefore: 1,
                statusBefore: Domain.ValueObjects.InventoryItemStatus.InStock,
                errorMessage: $"Reversal: Cancelled PPERR {report.RRNumber}");

            transactionLogs.Add(reversalLog);
        }

        // Save the cancelled report and reversal logs
        await repository.UpdateAsync(report, cancellationToken);
        foreach (var log in transactionLogs)
        {
            await transactionLogRepository.AddAsync(log, cancellationToken);
        }
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("PPERR {Id} cancelled successfully with {Count} reversal entries", request.Id, transactionLogs.Count);

        return new CancelPpeReceivingReportResponse(
            Id: report.Id,
            Status: report.Status.ToString(),
            Message: "PPE Receiving Report cancelled and reversal entries created");
    }
}
