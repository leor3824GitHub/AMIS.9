using System;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Specs;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Post.v1;

public sealed class PostPpeIssuanceReportHandler(
    ILogger<PostPpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PpeIssuanceReport> issuanceRepository,
    [FromKeyedServices("inventories:inventory-registries")] IRepository<InventoryRegistry> registryRepository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<InventoryTransactionLog> transactionLogRepository,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> employeeRepository)
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

            var employee = await ResolveRecipientEmployeeAsync(report.Recipient?.Name, cancellationToken).ConfigureAwait(false);
            if (employee == null)
            {
                logger.LogWarning("PPEIR {ReportNumber} has no resolvable recipient employee. Asset assignment history will be skipped.", report.ReportNumber);
            }

            foreach (var lineItem in report.LineItems)
            {
                // Issuance is per-item; domain line items do not carry quantity, so enforce 1
                var quantity = 1;
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
                        report.Recipient?.Name ?? "Unknown");

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
                        report.Recipient?.Name ?? "Unknown");


                    await transactionLogRepository.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);

                    if (employee != null)
                    {
                        var assetSpec = new AssetByPropertyCodeSpec(lineItem.PropertyCode);
                        var asset = await assetRepository.FirstOrDefaultAsync(assetSpec, cancellationToken).ConfigureAwait(false);
                        if (asset == null)
                        {
                            logger.LogWarning("PhysicalAsset not found for PropertyCode {PropertyCode} during PPEIR {ReportNumber} posting", lineItem.PropertyCode, report.ReportNumber);
                        }
                        else
                        {
                            asset.Issue(employee.Id, employee.Name, report.ReportNumber, quantityIssued: quantity, location: lineItem.Location, emitEvent: false);
                            await assetRepository.UpdateAsync(asset, cancellationToken).ConfigureAwait(false);
                        }
                    }
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
                        report.Recipient?.Name ?? "Unknown");

                    await transactionLogRepository.AddAsync(failureLog, cancellationToken).ConfigureAwait(false);
                    throw;
                }
            }

            await issuanceRepository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await assetRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await employeeRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
