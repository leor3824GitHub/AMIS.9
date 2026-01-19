using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Post.v1;

public sealed class PostPpeIssuanceReportHandler(
    ILogger<PostPpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PpeIssuanceReport> issuanceRepository,
    [FromKeyedServices("inventories:inventory-registries")] IRepository<InventoryRegistry> registryRepository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<InventoryTransactionLog> transactionLogRepository)
    : IRequestHandler<PostPpeIssuanceReportCommand, PostPpeIssuanceReportResponse>
{
    public async Task<PostPpeIssuanceReportResponse> Handle(PostPpeIssuanceReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = await issuanceRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"PPE Issuance Report with Id {request.Id} was not found.");
            }

            report.Post();

            foreach (var lineItem in report.LineItems)
            {
                var quantity = (int)1; // PPE issuance is typically per item
                if (quantity <= 0)
                {
                    throw new ArgumentException($"Quantity must be greater than zero for {lineItem.PropertyCode}");
                }

                var registrySpec = new InventoryRegistryByPropertyCodeSpec(lineItem.PropertyCode);
                var registry = await registryRepository.FirstOrDefaultAsync(registrySpec, cancellationToken).ConfigureAwait(false);
                if (registry is null)
                {
                    var failureLog = InventoryTransactionLog.CreateFailure(
                        lineItem.PropertyCode,
                        "PPEIR",
                        report.ReportNumber,
                        0,
                        Domain.ValueObjects.InventoryItemStatus.NotReceived,
                        $"Registry entry for property code '{lineItem.PropertyCode}' was not found.",
                        report.Recipient.Name);

                    await transactionLogRepository.AddAsync(failureLog, cancellationToken).ConfigureAwait(false);
                    throw new InvalidOperationException($"Cannot issue item. Registry entry for property code '{lineItem.PropertyCode}' was not found.");
                }

                var inventoryBefore = registry.Quantity;
                var statusBefore = registry.Status;

                try
                {
                    registry.DeductQuantity(quantity, report.ReportNumber);
                    await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);

                    var logEntry = InventoryTransactionLog.CreateSuccess(
                        lineItem.PropertyCode,
                        "PPEIR",
                        report.ReportNumber,
                        -quantity,
                        inventoryBefore,
                        registry.Quantity,
                        statusBefore,
                        registry.Status,
                        report.Recipient.Name);

                    await transactionLogRepository.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    var failureLog = InventoryTransactionLog.CreateFailure(
                        lineItem.PropertyCode,
                        "PPEIR",
                        report.ReportNumber,
                        inventoryBefore,
                        statusBefore,
                        ex.Message,
                        report.Recipient.Name);

                    await transactionLogRepository.AddAsync(failureLog, cancellationToken).ConfigureAwait(false);
                    throw;
                }
            }

            await issuanceRepository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await issuanceRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Issuance Report {ReportNumber} posted successfully. Registry and logs updated.", report.ReportNumber);
            return new PostPpeIssuanceReportResponse(report.Id, report.ReportNumber, report.Status.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting PPE Issuance Report {Id}.", request.Id);
            throw;
        }
    }
}
