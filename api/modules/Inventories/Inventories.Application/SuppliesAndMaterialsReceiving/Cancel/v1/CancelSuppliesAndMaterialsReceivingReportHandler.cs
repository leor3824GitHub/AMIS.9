using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Cancel.v1;

public sealed class CancelSuppliesAndMaterialsReceivingReportHandler(
    [FromKeyedServices("inventories:smrr")] IRepository<SuppliesAndMaterialsReceivingReport> repository,
    [FromKeyedServices("inventories:semex-registries")] IRepository<SemexRegistryDomain> registryRepository,
    [FromKeyedServices("inventories:semex-transaction-logs")] IRepository<SemexTransactionLogDomain> transactionLogRepository,
    IAuthorizationService authorizationService,
    ILogger<CancelSuppliesAndMaterialsReceivingReportHandler> logger)
    : IRequestHandler<CancelSuppliesAndMaterialsReceivingReportCommand, CancelSuppliesAndMaterialsReceivingReportResponse>
{
    public async Task<CancelSuppliesAndMaterialsReceivingReportResponse> Handle(
        CancelSuppliesAndMaterialsReceivingReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check: Only users with Cancel permission can cancel reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.SuppliesAndMaterialsReceiving}.{FshActions.Cancel}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized cancellation attempt for SMRR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to cancel Supplies and Materials Receiving reports. Only accounting personnel can perform this action.");
        }

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"SMRR with Id {request.Id} was not found.");
            }

            var transactionLogs = new List<SemexTransactionLogDomain>();
            var registries = await registryRepository.ListAsync(new AllSemexRegistriesSpec(), cancellationToken).ConfigureAwait(false);

            foreach (var lineItem in report.LineItems)
            {
                var normalizedCode = lineItem.Name.Trim().ToUpperInvariant();
                var registry = registries.FirstOrDefault(r => r.ItemCode == normalizedCode);

                if (registry is not null)
                {
                    var quantityBefore = registry.Quantity;
                    var statusBefore = registry.Status;

                    try
                    {
                        // Reverse the quantity
                        var quantity = (int)lineItem.Quantity;
                        registry.DeductQuantity(quantity, report.SmrrNumber);
                        await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);

                        // Create reversal log
                        var reversalLog = SemexTransactionLogDomain.CreateSuccess(
                            lineItem.Name,
                            "SMRR",
                            report.SmrrNumber,
                            -quantity,
                            quantityBefore,
                            registry.Quantity,
                            statusBefore,
                            registry.Status);

                        transactionLogs.Add(reversalLog);
                    }
                    catch (Exception ex)
                    {
                        var failureLog = SemexTransactionLogDomain.CreateFailure(
                            lineItem.Name,
                            "SMRR",
                            report.SmrrNumber,
                            quantityBefore,
                            statusBefore,
                            $"Reversal failed: {ex.Message}");

                        transactionLogs.Add(failureLog);
                        logger.LogWarning("Failed to reverse SMRR {SmrrNumber} for item {ItemCode}: {Error}",
                            report.SmrrNumber, lineItem.Name, ex.Message);
                    }
                }
                else
                {
                    var failureLog = SemexTransactionLogDomain.CreateFailure(
                        lineItem.Name,
                        "SMRR",
                        report.SmrrNumber,
                        0,
                        InventoryItemStatus.NotReceived,
                        $"Registry entry not found for reversal of SMRR {report.SmrrNumber}");

                    transactionLogs.Add(failureLog);
                    logger.LogWarning("Registry entry not found for item {ItemName} during SMRR {Id} cancellation",
                        lineItem.Name, request.Id);
                }
            }

            // Save the cancelled report and reversal logs
            await repository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            foreach (var log in transactionLogs)
            {
                await transactionLogRepository.AddAsync(log, cancellationToken).ConfigureAwait(false);
            }
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("SMRR {Id} cancelled successfully with {Count} reversal entries", request.Id, transactionLogs.Count);

            return new CancelSuppliesAndMaterialsReceivingReportResponse(
                report.Id,
                "Cancelled",
                "SMRR cancelled and reversal entries created");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cancelling SMRR {Id}.", request.Id);
            throw;
        }
    }
}
