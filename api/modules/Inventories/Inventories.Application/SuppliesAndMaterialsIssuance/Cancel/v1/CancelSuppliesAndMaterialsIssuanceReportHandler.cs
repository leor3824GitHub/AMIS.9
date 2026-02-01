using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Cancel.v1;

public sealed class CancelSuppliesAndMaterialsIssuanceReportHandler(
    [FromKeyedServices("inventories:smir")] IRepository<SuppliesAndMaterialsIssuanceReport> repository,
    [FromKeyedServices("inventories:semex-registries")] IRepository<SemexRegistryDomain> registryRepository,
    [FromKeyedServices("inventories:semex-transaction-logs")] IRepository<SemexTransactionLogDomain> transactionLogRepository,
    IAuthorizationService authorizationService,
    ILogger<CancelSuppliesAndMaterialsIssuanceReportHandler> logger)
    : IRequestHandler<CancelSuppliesAndMaterialsIssuanceReportCommand, CancelSuppliesAndMaterialsIssuanceReportResponse>
{
    public async Task<CancelSuppliesAndMaterialsIssuanceReportResponse> Handle(
        CancelSuppliesAndMaterialsIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check: Only users with Cancel permission can cancel reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.SuppliesAndMaterialsIssuance}.{FshActions.Cancel}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized cancellation attempt for SMIR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to cancel Supplies and Materials Issuance reports. Only accounting personnel can perform this action.");
        }

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
                var normalizedCode = lineItem.PropertyCode.Trim().ToUpperInvariant();
                var registry = registries.FirstOrDefault(r => r.ItemCode == normalizedCode);

                if (registry is not null)
                {
                    var quantityBefore = registry.Quantity;
                    var statusBefore = registry.Status;

                    try
                    {
                        // Reverse the quantity (add back)
                        var quantity = (int)lineItem.Quantity;
                        registry.AddQuantity(quantity, report.SmirNumber);
                        await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);

                        // Create reversal log
                        var reversalLog = SemexTransactionLogDomain.CreateSuccess(
                            lineItem.PropertyCode,
                            "SMIR",
                            report.SmirNumber,
                            quantity,
                            quantityBefore,
                            registry.Quantity,
                            statusBefore,
                            registry.Status);

                        transactionLogs.Add(reversalLog);
                    }
                    catch (Exception ex)
                    {
                        var failureLog = SemexTransactionLogDomain.CreateFailure(
                            lineItem.PropertyCode,
                            "SMIR",
                            report.SmirNumber,
                            quantityBefore,
                            statusBefore,
                            $"Reversal failed: {ex.Message}");

                        transactionLogs.Add(failureLog);
                        logger.LogWarning("Failed to reverse SMIR {SmirNumber} for property code {PropertyCode}: {Error}",
                            report.SmirNumber, lineItem.PropertyCode, ex.Message);
                    }
                }
                else
                {
                    var failureLog = SemexTransactionLogDomain.CreateFailure(
                        lineItem.PropertyCode,
                        "SMIR",
                        report.SmirNumber,
                        0,
                        InventoryItemStatus.NotReceived,
                        $"Registry entry not found for reversal of SMIR {report.SmirNumber}");

                    transactionLogs.Add(failureLog);
                    logger.LogWarning("Registry entry not found for property code {PropertyCode} during SMIR {Id} cancellation",
                        lineItem.PropertyCode, request.Id);
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

            logger.LogInformation("SMIR {Id} cancelled successfully with {Count} reversal entries", request.Id, transactionLogs.Count);

            return new CancelSuppliesAndMaterialsIssuanceReportResponse(
                report.Id,
                "Cancelled",
                "SMIR cancelled and reversal entries created");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cancelling SMIR {Id}.", request.Id);
            throw;
        }
    }
}
