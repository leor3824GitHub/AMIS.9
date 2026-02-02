using System;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Ardalis.Specification;
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
    [FromKeyedServices("inventories:semex-transaction-logs")] IRepository<SemexTransactionLogDomain> transactionLogRepository,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
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
            var employee = await ResolveRecipientEmployeeAsync(report.Recipient?.Name, cancellationToken).ConfigureAwait(false);
            if (employee == null)
            {
                logger.LogWarning("SMIR {SmirNumber} has no resolvable recipient employee. Asset assignment history will be skipped.", report.SmirNumber);
            }

            foreach (var lineItem in report.LineItems)
            {
                if (string.IsNullOrWhiteSpace(lineItem.PropertyCode))
                {
                    logger.LogWarning("Skipping SMIR line item with empty property code on report {SmirNumber}", report.SmirNumber);
                    continue;
                }

                var quantity = (int)lineItem.Quantity;
                if (quantity <= 0)
                {
                    throw new ArgumentException($"Quantity must be greater than zero for {lineItem.PropertyCode} - {lineItem.Name}");
                }

                // Use property code for registry lookup
                var normalizedCode = lineItem.PropertyCode.Trim().ToUpperInvariant();
                var registry = registries.FirstOrDefault(r => r.ItemCode == normalizedCode);

                if (registry is null)
                {
                    var failureLog = SemexTransactionLogDomain.CreateFailure(
                        lineItem.PropertyCode,
                        "SMIR",
                        report.SmirNumber,
                        0,
                        InventoryItemStatus.NotReceived,
                        $"Registry entry for property code '{lineItem.PropertyCode}' was not found.",
                        report.Recipient?.Name ?? "Unknown");

                    transactionLogs.Add(failureLog);
                    logger.LogWarning("Registry entry not found for property code {PropertyCode} during SMIR {Id} posting",
                        lineItem.PropertyCode, request.Id);
                    throw new InvalidOperationException($"Cannot issue item. Registry entry for property code '{lineItem.PropertyCode}' was not found.");
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

                    if (employee != null)
                    {
                        var assetSpec = new AssetByPropertyCodeSpec(lineItem.PropertyCode);
                        var asset = await assetRepository.FirstOrDefaultAsync(assetSpec, cancellationToken).ConfigureAwait(false);
                        if (asset == null)
                        {
                            logger.LogWarning("PhysicalAsset not found for PropertyCode {PropertyCode} during SMIR {SmirNumber} posting", lineItem.PropertyCode, report.SmirNumber);
                        }
                        else
                        {
                            asset.Issue(employee.Id, employee.Name, report.SmirNumber, quantityIssued: quantity, emitEvent: false);
                            await assetRepository.UpdateAsync(asset, cancellationToken).ConfigureAwait(false);
                        }
                    }
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
            await assetRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await employeeRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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

    private async Task<Employee?> ResolveRecipientEmployeeAsync(string? recipientName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(recipientName))
        {
            return null;
        }

        var normalizedName = recipientName.Trim();
        var employee = await employeeRepository.FirstOrDefaultAsync(new EmployeeByNameSpec(normalizedName), cancellationToken).ConfigureAwait(false);
        if (employee != null)
        {
            return employee;
        }

        employee = Employee.Create(normalizedName, "Unknown", "N/A", userId: null);
        await employeeRepository.AddAsync(employee, cancellationToken).ConfigureAwait(false);
        return employee;
    }

    private sealed class EmployeeByNameSpec : Specification<Employee>
    {
        public EmployeeByNameSpec(string name)
            => Query.Where(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}
