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
    [FromKeyedServices("inventories:pperr")] IRepository<PpeReceivingReport> receivingRepository,
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

            foreach (var lineItem in report.LineItems)
            {
                if (string.IsNullOrWhiteSpace(lineItem.Description)
                    && string.IsNullOrWhiteSpace(lineItem.Location))
                {
                    logger.LogWarning("Skipping PPERR line item with empty description/location on report {ReportNumber}", report.ReportNumber);
                    continue;
                }

                var quantity = (int)lineItem.Quantity;
                if (quantity <= 0)
                {
                    throw new ArgumentException($"Quantity must be greater than zero for {lineItem.Description}");
                }

                var productName = string.IsNullOrWhiteSpace(lineItem.Description) ? "PPE Item" : lineItem.Description.Trim();
                // var product = await productRepository.FirstOrDefaultAsync(new ProductByNameSpec(productName), cancellationToken).ConfigureAwait(false);
                Product? product = null;
                if (product is null)
                {
                    product = Product.Create(
                        name: productName,
                        description: lineItem.Description,
                        sku: lineItem.UnitCost,
                        unit: lineItem.Unit,
                        imagePath: null,
                        categoryId: null,
                        classification: PropertyClassification.PropertyPlantEquipment,
                        estimatedUsefulLife: 60);

                    await productRepository.AddAsync(product, cancellationToken).ConfigureAwait(false);
                }

                for (var i = 0; i < quantity; i++)
                {
                    var propertyCode = await propertyCodeGenerator.GenerateAsync(
                        new CoaPropertyCodeRequest(
                            lineItem.DateAcquired,
                            PropertyClassification.PropertyPlantEquipment,
                            OfficeCode: null,
                            ClassCode: lineItem.ClassCode,
                            CategoryCode: lineItem.CategoryCode,
                            ItemCode: lineItem.ItemCode,
                            SequenceSuffix: "0"),
                        cancellationToken).ConfigureAwait(false);

                    var asset = PhysicalAsset.Create(
                        PropertyClassification.PropertyPlantEquipment,
                        propertyCode,
                        product.Id,
                        lineItem.Description,
                        lineItem.UnitCost,
                        lineItem.DateAcquired,
                        estimatedUsefulLife: 60,
                        quantity: 1,
                        unitOfMeasure: lineItem.Unit,
                        serialNumber: null,
                        modelNumber: null,
                        ppeType: lineItem.PpeType ?? "General");

                    // Create initial assignment with location from PPERR line item
                    asset.Issue(
                        employeeId: Guid.Empty,
                        employeeName: report.Source.Name ?? "System",
                        documentNumber: report.ReportNumber,
                        quantityIssued: null,
                        location: lineItem.Location,
                        emitEvent: false);

                    await assetRepository.AddAsync(asset, cancellationToken).ConfigureAwait(false);

                    var registrySpec = new InventoryRegistryByPropertyCodeSpec(propertyCode);
                    var registry = await registryRepository.FirstOrDefaultAsync(registrySpec, cancellationToken).ConfigureAwait(false);

                    var inventoryBefore = registry?.Quantity ?? 0;
                    var statusBefore = registry?.Status ?? InventoryItemStatus.NotReceived;

                    if (registry is null)
                    {
                        registry = InventoryRegistry.CreateFromReceiving(
                            propertyCode,
                            lineItem.Description,
                            1,
                            lineItem.Location,
                            report.ReportNumber);

                        await registryRepository.AddAsync(registry, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        registry.AddQuantity(1, report.ReportNumber);
                        await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);
                    }

                    var logEntry = InventoryTransactionLog.CreateSuccess(
                        propertyCode,
                        "PPERR",
                        report.ReportNumber,
                        1,
                        inventoryBefore,
                        registry.Quantity,
                        statusBefore,
                        registry.Status,
                        report.Source.Name);

                    await transactionLogRepository.AddAsync(logEntry, cancellationToken).ConfigureAwait(false);
                }
            }

            await receivingRepository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await registryRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transactionLogRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await assetRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await productRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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

    // TODO: Fix Query.Where type inference issues with Ardalis Specification
    // private sealed class ProductByNameSpec : Ardalis.Specification.Specification<Product>
    // {
    //     public ProductByNameSpec(string name)
    //     {
    //         _ = base.Query.Where(p => p.Name.ToLower() == name.ToLower());
    //     }
    // }
}
