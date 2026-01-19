using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Cancel.v1;

public sealed class CancelPpeIssuanceReportHandler(
    [FromKeyedServices("inventories:ppeir")] IRepository<Domain.PpeIssuanceReport> repository,
    [FromKeyedServices("inventories:inventory-registries")] IRepository<Domain.InventoryRegistry> registryRepository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<Domain.InventoryTransactionLog> transactionLogRepository,
    ILogger<CancelPpeIssuanceReportHandler> logger) : IRequestHandler<CancelPpeIssuanceReportCommand, CancelPpeIssuanceReportResponse>
{
    public async Task<CancelPpeIssuanceReportResponse> Handle(CancelPpeIssuanceReportCommand request, CancellationToken cancellationToken)
    {
        var report = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (report is null)
        {
            logger.LogWarning("PPE Issuance Report {Id} not found for cancellation", request.Id);
            throw new InvalidOperationException($"PPE Issuance Report {request.Id} not found.");
        }

        // Only allow cancellation of Posted reports
        if (report.Status != Domain.ValueObjects.PpeReportStatus.Posted)
        {
            logger.LogWarning("Cannot cancel PPEIR {Id} with status {Status}", request.Id, report.Status);
            throw new InvalidOperationException($"Only Posted reports can be cancelled. Current status: {report.Status}");
        }

        // Cancel the report
        report.Cancel();

        var transactionLogs = new List<Domain.InventoryTransactionLog>();

        // For issuance (which deducted items), we reverse the deduction by adding back
        foreach (var lineItem in report.LineItems)
        {
            var registrySpec = new InventoryRegistryByPropertyCodeSpec(lineItem.PropertyCode);
            var registry = await registryRepository.FirstOrDefaultAsync(registrySpec, cancellationToken);

            if (registry is not null)
            {
                var quantityBefore = registry.Quantity;
                var statusBefore = registry.Status;

                // Add back 1 unit (reverse the issuance)
                registry.AddQuantity(1, report.ReportNumber);

                // Create reversal log
                var reversalLog = Domain.InventoryTransactionLog.CreateSuccess(
                    propertyCode: lineItem.PropertyCode,
                    transactionType: "PPEIR",
                    reportNumber: report.ReportNumber,
                    quantityChange: 1,
                    inventoryBefore: quantityBefore,
                    inventoryAfter: registry.Quantity,
                    statusBefore: statusBefore,
                    statusAfter: registry.Status);

                transactionLogs.Add(reversalLog);
                await registryRepository.UpdateAsync(registry, cancellationToken);
            }
            else
            {
                // Registry entry not found, log as failure
                var failureLog = Domain.InventoryTransactionLog.CreateFailure(
                    propertyCode: lineItem.PropertyCode,
                    transactionType: "PPEIR",
                    reportNumber: report.ReportNumber,
                    inventoryBefore: 0,
                    statusBefore: Domain.ValueObjects.InventoryItemStatus.NotReceived,
                    errorMessage: $"Registry entry not found for reversal of PPEIR {report.ReportNumber}");

                transactionLogs.Add(failureLog);
                logger.LogWarning("Registry entry not found for property code {PropertyCode} during PPEIR {Id} cancellation", lineItem.PropertyCode, request.Id);
            }
        }

        // Save the cancelled report and reversal logs
        await repository.UpdateAsync(report, cancellationToken);
        foreach (var log in transactionLogs)
        {
            await transactionLogRepository.AddAsync(log, cancellationToken);
        }
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("PPEIR {Id} cancelled successfully with {Count} reversal entries", request.Id, transactionLogs.Count);

        return new CancelPpeIssuanceReportResponse(
            Id: report.Id,
            Status: report.Status.ToString(),
            Message: "PPE Issuance Report cancelled and reversal entries created");
    }
}
