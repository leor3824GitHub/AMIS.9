using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Post.v1;

public sealed class PostSuppliesAndMaterialsIssuanceReportHandler(
    ILogger<PostSuppliesAndMaterialsIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:smir")] IRepository<SuppliesAndMaterialsIssuanceReport> repository,
    [FromKeyedServices("inventories:semex-registries")] IRepository<SemexRegistryDomain> registryRepository,
    [FromKeyedServices("inventories:semex-transaction-logs")] IRepository<SemexTransactionLogDomain> transactionLogRepository)
    : IRequestHandler<PostSuppliesAndMaterialsIssuanceReportCommand, PostSuppliesAndMaterialsIssuanceReportResponse>
{
    public async Task<PostSuppliesAndMaterialsIssuanceReportResponse> Handle(
        PostSuppliesAndMaterialsIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"SMIR with Id {request.Id} was not found.");
            }

            var transactionLogs = new List<SemexTransactionLogDomain>();
            var registries = await registryRepository.ListAsync(new AllSemexRegistriesSpec(), cancellationToken).ConfigureAwait(false);

            foreach (var lineItem in report.LineItems)
            {
                if (string.IsNullOrWhiteSpace(lineItem.Name)
                    && string.IsNullOrWhiteSpace(lineItem.Description))
                {
                    logger.LogWarning("Skipping SMIR line item with empty name/description on report {SmirNumber}", report.SmirNumber);
                    continue;
                }

                var quantity = (int)lineItem.Quantity;
                if (quantity <= 0)
                {
                    throw new ArgumentException($"Quantity must be greater than zero for {lineItem.Name}");
                }

                // Use normalized item code for lookup
                var normalizedCode = lineItem.Name.Trim().ToUpperInvariant();
                var registry = registries.FirstOrDefault(r => r.ItemCode == normalizedCode);

                if (registry is null)
                {
                    var failureLog = SemexTransactionLogDomain.CreateFailure(
                        lineItem.Name,
                        "SMIR",
                        report.SmirNumber,
                        0,
                        InventoryItemStatus.NotReceived,
                        $"Registry entry for item '{lineItem.Name}' was not found.",
                        report.Recipient?.Name ?? "Unknown");

                    transactionLogs.Add(failureLog);
                    logger.LogWarning("Registry entry not found for item {ItemName} during SMIR {Id} posting",
                        lineItem.Name, request.Id);
                    throw new InvalidOperationException($"Cannot issue item. Registry entry for item '{lineItem.Name}' was not found.");
                }

                var inventoryBefore = registry.Quantity;
                var statusBefore = registry.Status;

                try
                {
                    registry.DeductQuantity(quantity, report.SmirNumber);
                    await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);

                    var logEntry = SemexTransactionLogDomain.CreateSuccess(
                        lineItem.Name,
                        "SMIR",
                        report.SmirNumber,
                        -quantity,
                        inventoryBefore,
                        registry.Quantity,
                        statusBefore,
                        registry.Status,
                        report.Recipient?.Name ?? "Unknown");

                    transactionLogs.Add(logEntry);
                }
                catch (Exception ex)
                {
                    var failureLog = SemexTransactionLogDomain.CreateFailure(
                        lineItem.Name,
                        "SMIR",
                        report.SmirNumber,
                        inventoryBefore,
                        statusBefore,
                        ex.Message,
                        report.Recipient?.Name ?? "Unknown");

                    transactionLogs.Add(failureLog);
                    logger.LogError(ex, "Error deducting quantity for item {ItemName} during SMIR {Id} posting",
                        lineItem.Name, request.Id);
                    throw;
                }
            }

            // Save all changes
            await repository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            foreach (var log in transactionLogs)
            {
                await transactionLogRepository.AddAsync(log, cancellationToken).ConfigureAwait(false);
            }
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("SMIR {SmirNumber} posted successfully. Semex registry and logs updated.", report.SmirNumber);
            return new PostSuppliesAndMaterialsIssuanceReportResponse(
                report.Id,
                report.SmirNumber,
                "Posted",
                "SMIR posted and semi-expendable inventory registry updated.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting SMIR {Id}.", request.Id);
            throw;
        }
    }
}
