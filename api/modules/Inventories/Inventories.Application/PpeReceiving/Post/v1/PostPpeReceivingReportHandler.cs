using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.Acceptances.Services;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using AMIS.WebApi.Inventories.Application.PropertyCodes;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Post.v1;

public sealed class PostPpeReceivingReportHandler(
    ILogger<PostPpeReceivingReportHandler> logger,
    [FromKeyedServices("inventories:pperr")] IRepository<PPERR> receivingRepository,
    [FromKeyedServices("inventories:inventory-registries")] IRepository<InventoryRegistry> registryRepository,
    [FromKeyedServices("inventories:inventory-transaction-logs")] IRepository<InventoryTransactionLog> transactionLogRepository,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:products")] IRepository<Product> productRepository,
    IAssetPropertyCodeGenerator propertyCodeGenerator)
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

            foreach (var lineItem in report.Items)
            {
                var propertyCode = lineItem.PropertyCode;
                if (string.IsNullOrWhiteSpace(propertyCode))
                {
                    logger.LogWarning("Skipping PPERR line item with empty property code on report {RRNumber}", report.RRNumber);
                    continue;
                }

                var registrySpec = new InventoryRegistryByPropertyCodeSpec(propertyCode);
                var registry = await registryRepository.FirstOrDefaultAsync(registrySpec, cancellationToken).ConfigureAwait(false);

                if (registry is not null)
                {
                    var inventoryBefore = registry.Quantity;
                    var statusBefore = registry.Status;

                    registry.AddQuantity(1, report.RRNumber);
                    await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);

                    var logEntry = InventoryTransactionLog.CreateSuccess(
                        propertyCode,
                        "PPERR",
                        report.RRNumber,
                        1,
                        inventoryBefore,
                        registry.Quantity,
                        statusBefore,
                        registry.Status,
                        report.ReceivedFrom);

                    await transactionLogRepository.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    logger.LogWarning("Property code {PropertyCode} not found in inventory registry for PPERR {RRNumber}", propertyCode, report.RRNumber);
                }
            }

            await receivingRepository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await receivingRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Receiving Report {RRNumber} posted successfully. Registry and logs updated.", report.RRNumber);
            return new PostPpeReceivingReportResponse(report.Id, report.RRNumber, report.Status.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting PPE Receiving Report {Id}.", request.Id);
            throw;
        }
    }

    // TODO: Fix Query.Where type inference issues with Ardalis Specification
    // private sealed class ProductByNameSpec : Ardalis.Specification.Specification<Product>
    // {
    //     public ProductByNameSpec(string name)
    //     {
    //         _ = base.Query.Where(p => p.Name.ToLower() == name.ToLower());
    //     }
    // }
}
