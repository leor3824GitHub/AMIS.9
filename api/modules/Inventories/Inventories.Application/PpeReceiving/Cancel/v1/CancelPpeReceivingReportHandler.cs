using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Cancel.v1;

public sealed class CancelPpeReceivingReportHandler(
    [FromKeyedServices("inventories:pperr")] IRepository<Domain.PpeReceivingReport> repository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<Domain.InventoryTransactionLog> transactionLogRepository,
    ILogger<CancelPpeReceivingReportHandler> logger) : IRequestHandler<CancelPpeReceivingReportCommand, CancelPpeReceivingReportResponse>
{
    public async Task<CancelPpeReceivingReportResponse> Handle(CancelPpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
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
        foreach (var lineItem in report.LineItems)
        {
            var reversalLog = Domain.InventoryTransactionLog.CreateFailure(
                propertyCode: lineItem.PropertyCode,
                transactionType: "PPERR",
                reportNumber: report.ReportNumber,
                inventoryBefore: (int)lineItem.Quantity,
                statusBefore: Domain.ValueObjects.InventoryItemStatus.InStock,
                errorMessage: $"Reversal: Cancelled PPERR {report.ReportNumber}");

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
