using System;
using System.Collections.Generic;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Services;

/// <summary>
/// Application-layer factory that turns posted Acceptances into PhysicalAsset instances.
/// Delegates property code and classification decisions to caller-provided resolvers so infrastructure/config lookups stay outside the domain.
/// </summary>
public static class AcceptanceAssetFactory
{
    public static async Task<IReadOnlyCollection<PhysicalAsset>> CreateAssetsAsync(
        Acceptance acceptance,
        IAssetPropertyCodeGenerator codeGenerator,
        IAssetClassificationResolver classificationResolver,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(acceptance);
        if (!acceptance.IsPosted) throw new InvalidOperationException("Acceptance must be posted before creating assets.");
        if (acceptance.Items.Count == 0) throw new InvalidOperationException("Acceptance has no items to convert to assets.");

        var assets = new List<PhysicalAsset>();

        foreach (var acceptanceItem in acceptance.Items)
        {
            var purchaseItem = acceptanceItem.PurchaseItem;
            if (purchaseItem is null)
                throw new InvalidOperationException("Acceptance item is missing its PurchaseItem navigation.");

            if (!purchaseItem.ProductId.HasValue)
                throw new InvalidOperationException("Purchase item must have an associated product to create an asset.");

            var classification = classificationResolver.ResolveClassification(purchaseItem);
            var ppeType = classificationResolver.ResolvePpeType(purchaseItem);
            var estimatedUsefulLife = classificationResolver.ResolveEstimatedUsefulLifeMonths(purchaseItem);
            var description = classificationResolver.ResolveDescription(purchaseItem);
            var unitOfMeasure = classificationResolver.ResolveUnitOfMeasure(purchaseItem);

            if (classification == PropertyClassification.PropertyPlantEquipment)
            {
                for (var i = 0; i < acceptanceItem.QtyAccepted; i++)
                {
                    var propertyCode = await codeGenerator.GenerateAsync(acceptanceItem, cancellationToken).ConfigureAwait(false);
                    var asset = PhysicalAsset.Create(
                        classification,
                        propertyCode,
                        purchaseItem.ProductId.Value,
                        description,
                        purchaseItem.UnitPrice,
                        acceptance.AcceptanceDate,
                        estimatedUsefulLife,
                        quantity: 1,
                        unitOfMeasure: unitOfMeasure,
                        serialNumber: null,
                        modelNumber: null,
                        ppeType: ppeType);

                    assets.Add(asset);
                }
            }
            else
            {
                var propertyCode = await codeGenerator.GenerateAsync(acceptanceItem, cancellationToken).ConfigureAwait(false);
                var asset = PhysicalAsset.Create(
                    classification,
                    propertyCode,
                    purchaseItem.ProductId.Value,
                    description,
                    purchaseItem.UnitPrice * acceptanceItem.QtyAccepted,
                    acceptance.AcceptanceDate,
                    estimatedUsefulLife,
                    acceptanceItem.QtyAccepted,
                    unitOfMeasure,
                    serialNumber: null,
                    modelNumber: null,
                    ppeType: ppeType);

                assets.Add(asset);
            }
        }

        return assets;
    }
}

