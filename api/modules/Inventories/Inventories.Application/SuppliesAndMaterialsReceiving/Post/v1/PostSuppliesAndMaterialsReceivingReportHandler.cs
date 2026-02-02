using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.Acceptances.Services;
using AMIS.WebApi.Inventories.Application.PropertyCodes;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Post.v1;

public sealed class PostSuppliesAndMaterialsReceivingReportHandler(
    ILogger<PostSuppliesAndMaterialsReceivingReportHandler> logger,
    [FromKeyedServices("inventories:smrr")] IRepository<SuppliesAndMaterialsReceivingReport> repository,
    [FromKeyedServices("inventories:semex-registries")] IRepository<SemexRegistryDomain> registryRepository,
    [FromKeyedServices("inventories:semex-transaction-logs")] IRepository<SemexTransactionLogDomain> transactionLogRepository,
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> assetRepository,
    [FromKeyedServices("inventories:products")] IRepository<Product> productRepository,
    [FromKeyedServices("inventories:classificationRules")] IReadRepository<AssetClassificationRule> classificationRuleRepository,
    IAssetPropertyCodeGenerator propertyCodeGenerator)
    : IRequestHandler<PostSuppliesAndMaterialsReceivingReportCommand, PostSuppliesAndMaterialsReceivingReportResponse>
{
    public async Task<PostSuppliesAndMaterialsReceivingReportResponse> Handle(
        PostSuppliesAndMaterialsReceivingReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"SMRR with Id {request.Id} was not found.");
            }

            var transactionLogs = new List<SemexTransactionLogDomain>();
            // var classificationRules = await classificationRuleRepository.ListAsync(new ActiveClassificationRulesSpec(), cancellationToken).ConfigureAwait(false);
            var classificationRules = new List<AssetClassificationRule>();

            foreach (var lineItem in report.LineItems)
            {
                if (string.IsNullOrWhiteSpace(lineItem.Name)
                    && string.IsNullOrWhiteSpace(lineItem.Description)
                    && string.IsNullOrWhiteSpace(lineItem.Location))
                {
                    logger.LogWarning("Skipping SMRR line item with empty name/description/location on report {SmrrNumber}", report.SmrrNumber);
                    continue;
                }

                var quantity = (int)lineItem.Quantity;
                if (quantity <= 0)
                {
                    throw new ArgumentException($"Quantity must be greater than zero for {lineItem.Name}");
                }

                // Use normalized item code for lookup
                var normalizedCode = lineItem.Name.Trim().ToUpperInvariant();
                
                // Query all registries and find by item code (since there's no built-in spec)
                var spec = new AllSemexRegistriesSpec();
                var registries = await registryRepository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
                var registry = registries.FirstOrDefault(r => r.ItemCode == normalizedCode);

                var inventoryBefore = registry?.Quantity ?? 0;
                var statusBefore = registry?.Status ?? InventoryItemStatus.NotReceived;

                if (registry is null)
                {
                    // Create new registry entry
                    registry = SemexRegistryDomain.CreateFromReceiving(
                        lineItem.Name,
                        lineItem.Description,
                        quantity,
                        lineItem.Unit,
                        lineItem.Location,
                        lineItem.UnitCost,
                        report.SmrrNumber);

                    await registryRepository.AddAsync(registry, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    // Add to existing registry entry
                    registry.AddQuantity(quantity, report.SmrrNumber);
                    await registryRepository.UpdateAsync(registry, cancellationToken).ConfigureAwait(false);
                }

                var logEntry = SemexTransactionLogDomain.CreateSuccess(
                    lineItem.Name,
                    "SMRR",
                    report.SmrrNumber,
                    quantity,
                    inventoryBefore,
                    registry.Quantity,
                    statusBefore,
                    registry.Status,
                    report.Source.Name);

                transactionLogs.Add(logEntry);

                var classification = ResolveClassification(classificationRules, lineItem.UnitCost, lineItem.AcquisitionDate);
                if (classification == PropertyClassification.SemiExpendable)
                {
                    var productName = string.IsNullOrWhiteSpace(lineItem.Name) ? "Semi-Expendable Item" : lineItem.Name.Trim();
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
                            classification: PropertyClassification.SemiExpendable,
                            estimatedUsefulLife: 12);

                        await productRepository.AddAsync(product, cancellationToken).ConfigureAwait(false);
                    }

                    var propertyCode = await propertyCodeGenerator.GenerateAsync(
                        new CoaPropertyCodeRequest(
                            lineItem.AcquisitionDate,
                            PropertyClassification.SemiExpendable,
                            OfficeCode: null,
                            ClassCode: lineItem.ClassCode,
                            CategoryCode: lineItem.CategoryCode,
                            ItemCode: lineItem.ItemCode,
                            SequenceSuffix: "0"),
                        cancellationToken).ConfigureAwait(false);

                    var asset = PhysicalAsset.Create(
                        PropertyClassification.SemiExpendable,
                        propertyCode,
                        product.Id,
                        lineItem.Description,
                        lineItem.UnitCost * quantity,
                        lineItem.AcquisitionDate,
                        estimatedUsefulLife: 12,
                        quantity: quantity,
                        unitOfMeasure: lineItem.Unit,
                        serialNumber: null,
                        modelNumber: null,
                        ppeType: null);

                    await assetRepository.AddAsync(asset, cancellationToken).ConfigureAwait(false);
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
            await productRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("SMRR {SmrrNumber} posted successfully. Semex registry and logs updated.", report.SmrrNumber);
            return new PostSuppliesAndMaterialsReceivingReportResponse(
                report.Id,
                report.SmrrNumber,
                "Posted",
                "SMRR posted and semi-expendable inventory registry updated.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting SMRR {Id}.", request.Id);
            throw;
        }
    }

    private static PropertyClassification ResolveClassification(
        IReadOnlyCollection<AssetClassificationRule> rules,
        decimal unitCost,
        DateTime acquisitionDate)
    {
        var applicableRule = rules
            .Where(r => r.IsEffectiveOn(acquisitionDate) && r.AppliesToAsset(unitCost))
            .OrderByDescending(r => r.Priority)
            .FirstOrDefault();

        return applicableRule?.Classification ?? PropertyClassification.Consumable;
    }

    // TODO: Fix Query.Where type inference issues with Ardalis Specification
    // private sealed class ActiveClassificationRulesSpec : Ardalis.Specification.Specification<AssetClassificationRule>
    // {
    //     public ActiveClassificationRulesSpec()
    //     {
    //         _ = base.Query.Where(r => r.IsActive);
    //     }
    // }

    // private sealed class ProductByNameSpec : Ardalis.Specification.Specification<Product>
    // {
    //     public ProductByNameSpec(string name)
    //     {
    //         _ = base.Query.Where(p => p.Name.ToLower() == name.ToLower());
    //     }
    // }
}
