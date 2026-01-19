using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Post.v1;

public sealed class PostPpeReceivingReportHandler(
    ILogger<PostPpeReceivingReportHandler> logger,
    [FromKeyedServices("inventories:pperr")] IRepository<PpeReceivingReport> receivingRepository,
    [FromKeyedServices("inventories:inventory-registries")] IRepository<InventoryRegistry> registryRepository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<InventoryTransactionLog> transactionLogRepository)
    : IRequestHandler<PostPpeReceivingReportCommand, PostPpeReceivingReportResponse>
{
    public async Task<PostPpeReceivingReportResponse> Handle(PostPpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = await receivingRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"PPE Receiving Report with Id {request.Id} was not found.");
            }

            report.Post();

            foreach (var lineItem in report.LineItems)
            {
                var quantity = (int)lineItem.Quantity;
                if (quantity <= 0)
                {
                    throw new ArgumentException($"Quantity must be greater than zero for {lineItem.PropertyCode}");
                }

                var registrySpec = new InventoryRegistryByPropertyCodeSpec(lineItem.PropertyCode);
                var registry = await registryRepository.FirstOrDefaultAsync(registrySpec, cancellationToken).ConfigureAwait(false);

                var inventoryBefore = registry?.Quantity ?? 0;
                var statusBefore = registry?.Status ?? InventoryItemStatus.NotReceived;

                if (registry is null)
                {
                    registry = InventoryRegistry.CreateFromReceiving(
                        lineItem.PropertyCode,
                        lineItem.Description,
                        quantity,
                        report.Location,
                        report.ReportNumber);

                    await registryRepository.AddAsync(registry, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    registry.AddQuantity(quantity, report.ReportNumber);
                    await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);
                }

                var logEntry = InventoryTransactionLog.CreateSuccess(
                    lineItem.PropertyCode,
                    "PPERR",
                    report.ReportNumber,
                    quantity,
                    inventoryBefore,
                    registry.Quantity,
                    statusBefore,
                    registry.Status,
                    report.Source.Name);

                await transactionLogRepository.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);
            }

            await receivingRepository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await receivingRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Receiving Report {ReportNumber} posted successfully. Registry and logs updated.", report.ReportNumber);
            return new PostPpeReceivingReportResponse(report.Id, report.ReportNumber, report.Status.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting PPE Receiving Report {Id}.", request.Id);
            throw;
        }
    }
}
