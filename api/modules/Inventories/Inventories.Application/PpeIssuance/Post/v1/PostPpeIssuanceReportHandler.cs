using System;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Post.v1;

public sealed class PostPpeIssuanceReportHandler(
    ILogger<PostPpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PPEIR> issuanceRepository,
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

            foreach (var lineItem in report.Items)
            {
                var propertyCode = lineItem.PropertyCode;
                if (string.IsNullOrWhiteSpace(propertyCode))
                {
                    logger.LogWarning("Skipping PPEIR line item with empty property code on report {IRNumber}", report.IRNumber);
                    continue;
                }

                var registrySpec = new InventoryRegistryByPropertyCodeSpec(propertyCode);
                var registry = await registryRepository.FirstOrDefaultAsync(registrySpec, cancellationToken).ConfigureAwait(false);

                if (registry is not null)
                {
                    var inventoryBefore = registry.Quantity;
                    var statusBefore = registry.Status;

                    registry.DeductQuantity(1, report.IRNumber);
                    await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);

                    var logEntry = InventoryTransactionLog.CreateSuccess(
                        propertyCode,
                        "PPEIR",
                        report.IRNumber,
                        -1,
                        inventoryBefore,
                        registry.Quantity,
                        statusBefore,
                        registry.Status,
                        report.IssuedTo);

                    await transactionLogRepository.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    var failureLog = InventoryTransactionLog.CreateFailure(
                        propertyCode,
                        "PPEIR",
                        report.IRNumber,
                        0,
                        Domain.ValueObjects.InventoryItemStatus.NotReceived,
                        $"Registry entry for property code '{propertyCode}' was not found.",
                        report.IssuedTo);

                    await transactionLogRepository.AddAsync(failureLog, cancellationToken).ConfigureAwait(false);
                    throw new InvalidOperationException($"Cannot issue item. Registry entry for property code '{propertyCode}' was not found.");
                }
            }

            await issuanceRepository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await issuanceRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Issuance Report {IRNumber} posted successfully. Registry and logs updated.", report.IRNumber);
            return new PostPpeIssuanceReportResponse(report.Id, report.IRNumber, report.Status.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting PPE Issuance Report {Id}.", request.Id);
            throw;
        }
    }
}
